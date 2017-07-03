using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindowCleaner
{

	public class GondolaController : MonoBehaviour {

		public GameObject GondolaTop;
		public GameObject GondolaBottom;
		public LineRenderer leftCable;
		public LineRenderer rightCable;

		public CharacterController characterController;

		public float udSpeed = 0.1f;
		public float lrSpeed = 0.1f;

		string rightStickHorizontalAxis = "joystick {0} Right Horizontal";
		string rightStickVerticalAxis = "joystick {0} Right Vertical";

		public void SetPlayerNumber(int playerNumber)
		{
			rightStickHorizontalAxis = string.Format (rightStickHorizontalAxis, playerNumber);
			rightStickVerticalAxis = string.Format (rightStickVerticalAxis, playerNumber);
		}

		void Update () 
		{
			Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

			if(pos.x < 0.0) Debug.Log("I am left of the camera's view.");
			if(1.0 < pos.x) Debug.Log("I am right of the camera's view.");
			if(pos.y < 0.0) Debug.Log("I am below the camera's view.");
			if(1.0 < pos.y) Debug.Log("I am above the camera's view.");
			
			if (!characterController.isDisabled)
			{
				float lr = Input.GetAxis (rightStickHorizontalAxis);
				float ud = Input.GetAxis (rightStickVerticalAxis);

				GondolaBottom.transform.Translate (lr * lrSpeed, ud * udSpeed, 0);
				GondolaTop.transform.Translate (lr * lrSpeed, 0, 0);


				//if(!(os.x < 0.0) && !())

				Vector3 topPos = GondolaTop.transform.position;
				Vector3 btmPos = GondolaBottom.transform.position;

				leftCable.SetPosition (0, new Vector3 (topPos.x - 1, topPos.y, topPos.z)); 
				leftCable.SetPosition (1, new Vector3 (btmPos.x - 1, btmPos.y, btmPos.z)); 

				rightCable.SetPosition (0, new Vector3 (topPos.x + 1, topPos.y, topPos.z)); 
				rightCable.SetPosition (1, new Vector3 (btmPos.x + 1, btmPos.y, btmPos.z)); 
			}
				
		
		}



			
	}


}