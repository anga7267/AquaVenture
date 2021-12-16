using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	public float forwardSpeed = 30.0f;
	public float smartphoneRotSpeed = 0.01f;
	public float keyboardRotSpeed = 0.2f;

	float rotX = 0f;
	float rotY = 0f;
	float centerX = 0f;
	float centerY = 0f;

	bool paused;
	Rigidbody body;

	void Start()
	{
		Controller.motionHandle = this.HandleTransfer;
		Controller.pressHandle = this.HandleTransfer;
		paused = true;
		body = GetComponent<Rigidbody>();
	}

	public void HandleTransfer(Motion motion)
	{
		rotX = (motion.beta < 0f) ? 360f + motion.beta : motion.beta;
		rotY = motion.alpha;
	}
	public void HandleTransfer(Press press)
	{
		switch (press.id)
		{
			case 1:
				centerX = rotX;
				centerY = rotY;
				break;
			case 2:
				paused = !paused;
				break;
			default:
				Debug.Log("press: " + press.id.ToString());
				break;
		}
	}

	void Update()
	{
		if (Input.GetButtonDown("Pause"))
		{
			paused = !paused;
		}
		var axis = Quaternion.Inverse(body.rotation) * Vector3.up;
		if (rotX == 0f && rotY == 0f)
		{
			var x = Input.GetAxis("Vertical") * (Global.settings.invertUp ? 1f : -1f);
			var y = Input.GetAxis("Horizontal");
			body.MoveRotation(body.rotation * Quaternion.Euler(x * keyboardRotSpeed, 0f, 0f));
			body.MoveRotation(body.rotation * Quaternion.AngleAxis(y * keyboardRotSpeed, axis));
		}
		else
		{
			var x = centerX - rotX;
			var y = centerY - rotY;
			if (x < 0f)
			{
				x += 360f;
			}
			if (y < 0f)
			{
				y += 360f;
			}
			if (x > 180f)
			{
				x -= 360f;
			}
			if (y > 180f)
			{
				y -= 360f;
			}
			body.MoveRotation(body.rotation * Quaternion.Euler(x * smartphoneRotSpeed, 0f, 0f));
			body.MoveRotation(body.rotation * Quaternion.AngleAxis(y * smartphoneRotSpeed, axis));
		}

		if (paused == false)
		{
			body.velocity = body.transform.forward * forwardSpeed;
		}
		else
		{
			body.velocity = Vector3.zero;
		}
	}
}
