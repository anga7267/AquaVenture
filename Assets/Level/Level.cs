using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Level : MonoBehaviour
{
	GameObject[] rooms;

	Vector3 currentPosition;
	Quaternion currentRotation;
	Transform[] currentRooms;

	public GameObject player;

	void Start()
	{
		//get all children as possible paths
		rooms = new GameObject[transform.childCount];
		currentRooms = new Transform[5];
		for (var i = 0; i < rooms.Length; i++)
		{
			rooms[i] = transform.GetChild(i).gameObject;
		}
		//add random rooms together
		currentPosition = Vector3.zero;
		currentRotation = Quaternion.identity;
		for (var i = 0; i < currentRooms.Length; i++)
		{
			var idx = Random.Range(0, rooms.Length);
			var p = Instantiate(rooms[idx], currentPosition, currentRotation, transform);
			var x = p.transform.Find("door");
			p.SetActive(true);
			currentPosition = x.position;
			currentRotation = x.rotation;
			currentRooms[i] = x;
		}

	}

	void Update()
	{
		var door = currentRooms[0];
		var conn = player.transform.position - door.position;
		var angle = Vector3.Dot(door.forward, conn.normalized);
		var dist = angle * conn.magnitude;
		if (dist > 0)
		{
			Destroy(door.parent.gameObject);
			for (var i = 1; i < currentRooms.Length; i++)
			{
				currentRooms[i - 1] = currentRooms[i];
			}
			var idx = Random.Range(0, rooms.Length);
			var p = Instantiate(rooms[idx], currentPosition, currentRotation, transform);
			var x = p.transform.Find("door");
			p.SetActive(true);
			currentPosition = x.position;
			currentRotation = x.rotation;
			currentRooms[currentRooms.Length - 1] = x;
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Level))]
public class LevelScriptEditor : Editor
{

	Level level;

	GameObject[] paths;

	public override void OnInspectorGUI()
	{
		level = (Level)target;
		DrawDefaultInspector();

		if (GUILayout.Button("Update"))
		{
			while (level.transform.childCount > 0)
			{
				DestroyImmediate(level.transform.GetChild(0).gameObject);
			}

			var prefabs = System.IO.Directory.GetFiles(
				"Assets\\Level\\Rooms",
				"*.prefab",
				System.IO.SearchOption.AllDirectories
			);
			foreach (var name in prefabs)
			{
				var sep = name.Split(new char[] { '\\', '.' });
				var fab = PrefabUtility.LoadPrefabContents(name);
				fab.name = sep[sep.Length - 2];
				fab.SetActive(false);
				fab.transform.SetParent(level.transform);
			}
		}
	}
}
#endif
