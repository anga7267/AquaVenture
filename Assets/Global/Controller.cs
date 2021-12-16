using System.Collections;
using Unity.WebRTC;
using UnityEngine;
using UnityEngine.Networking;

public static class Controller
{
	static RTCPeerConnection connection;
	static RTCDataChannel channel;

	public delegate void MotionHandle(Motion motion);
	public delegate void PressHandle(Press press);
	public delegate void CodeHandle(string code);
	public delegate void ResultHandle(bool success);

	public static MotionHandle motionHandle = null;
	public static PressHandle pressHandle = null;

	public static void Init()
	{
		WebRTC.Initialize();
	}

	public static IEnumerator StartRTC(CodeHandle codeHandle)
	{
		// Create local peer
		var options = new RTCConfiguration
		{
			iceServers = new RTCIceServer[] { new RTCIceServer{
				urls = new string[] { "stun:stun.stunprotocol.org" }
			} },
		};
		connection = new RTCPeerConnection(ref options);
		channel = connection.CreateDataChannel("chat");
		channel.OnOpen = () => { Debug.Log("channel open"); };
		channel.OnClose = () => { Debug.Log("channel close"); };
		channel.OnMessage = (bytes) =>
		{
			var str = System.Text.Encoding.UTF8.GetString(bytes);
			var t = str[0];
			var data = str.Substring(1);
			switch (t)
			{
				case 'm':
				case 'M':
					if (motionHandle != null)
					{
						motionHandle(JsonUtility.FromJson<Motion>(data));
					}
					break;
				case 'p':
				case 'P':
					if (pressHandle != null)
					{
						pressHandle(JsonUtility.FromJson<Press>(data));
					}
					break;
				default:
					Debug.Log("could not handle message \"" + data + "\" with type \"" + t + "\"");
					break;
			}
		};
		var offer = connection.CreateOffer();
		yield return offer;
		var d = offer.Desc;
		yield return connection.SetLocalDescription(ref d);

		var form = new WWWForm();
		form.AddField("content", JsonUtility.ToJson(offer.Desc).Replace("\"type\":0", "\"type\":\"offer\""));
		form.AddField("expiry_days", "1");
		var request = UnityWebRequest.Post("https://dpaste.com/api/", form);
		yield return request.SendWebRequest();
		var url = request.downloadHandler.text.Split('/');
		codeHandle(url[url.Length - 1].Trim());
	}


	public static IEnumerator AnswerRTC(string code, ResultHandle resultHandle)
	{
		var request = UnityWebRequest.Get("https://dpaste.com/" + code + ".txt");
		yield return request.SendWebRequest();
		var str = request.downloadHandler.text;
		if (str[0] != '{')
		{
			resultHandle(false);
			yield break;
		}
		else
		{
			str = str.Replace("\"type\":\"answer\"", "\"type\":2");
			var answer = JsonUtility.FromJson<RTCSessionDescription>(str);
			yield return connection.SetRemoteDescription(ref answer);
			resultHandle(true);
		}
	}

	public static void Finish()
	{
		channel.Close();
		connection.Close();
		WebRTC.Dispose();
	}
}

public struct Motion
{
	public float alpha;
	public float beta;
	public float gamma;
	public bool absolute;
}

public struct Press
{
	public int id;
}
