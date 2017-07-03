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
			if (!characterController.isDisabled)
			{
				Vector3 oldTopPos = GondolaTop.transform.position;
				Vector3 oldBtmPos = GondolaBottom.transform.position;

				float lr = Input.GetAxis (rightStickHorizontalAxis);
				float ud = Input.GetAxis (rightStickVerticalAxis);

				GondolaBottom.transform.Translate (lr * lrSpeed, ud * udSpeed, 0);
				GondolaTop.transform.Translate (lr * lrSpeed, 0, 0);

				Vector3 topPos = GondolaTop.transform.position;
				Vector3 btmPos = GondolaBottom.transform.position;

				Vector3 pos = Camera.main.WorldToViewportPoint(GondolaTop.transform.position);

				if (pos.x < 0.0 || 1.0 < pos.x) {
					GondolaTop.transform.position = oldTopPos;
					GondolaBottom.transform.position = oldBtmPos;
				}

				leftCable.SetPosition (0, new Vector3 (topPos.x - 1, topPos.y, topPos.z)); 
				leftCable.SetPosition (1, new Vector3 (btmPos.x - 1, btmPos.y, btmPos.z)); 

				rightCable.SetPosition (0, new Vector3 (topPos.x + 1, topPos.y, topPos.z)); 
				rightCable.SetPosition (1, new Vector3 (btmPos.x + 1, btmPos.y, btmPos.z)); 
			}

		
		
				
		
		}



			
	}


}