using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GondolaController : MonoBehaviour {

	public GameObject GondolaTop;
	public GameObject GondolaBottom;
	public GameObject leftCable;
	public GameObject rightCable;

	public float udSpeed = 0.1f;
	public float lrSpeed = 0.1f;

	string rightStickHorizontalAxis = "joystick {0} Right Horizontal";
	string rightStickVerticalAxis = "joystick {0} Right Vertical";

	public void SetPlayerNumber(int playerNumber)
	{
		rightStickHorizontalAxis = string.Format (rightStickHorizontalAxis, playerNumber);
		rightStickVerticalAxis = string.Format (rightStickVerticalAxis, playerNumber);
	}

	void FixedUpdate () 
	{
		float lr = Input.GetAxis (rightStickHorizontalAxis);
		float ud = Input.GetAxis (rightStickVerticalAxis);

		GondolaBottom.transform.Translate(lr * lrSpeed, ud * udSpeed, 0);
		GondolaTop.transform.Translate (lr * lrSpeed, 0, 0);
	
	}
		
}
