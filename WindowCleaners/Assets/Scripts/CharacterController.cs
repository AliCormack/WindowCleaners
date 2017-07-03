using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WindowCleaner
{

	public class CharacterController : MonoBehaviour {

		public float speed = 10f;
		public float jumpHeight = 10f;

		public Color color;

		Rigidbody2D rigidBody;
		Collider2D myCollider;

		bool cleaning;
		bool isGrounded;

		public int cleanedWindows = 0;

		public int PlayerNumber{ get; private set; }

		string leftStickHorizontalAxis = "joystick {0} Left Horizontal";
		string jumpButton = "joystick {0} button 1";
		string cleanButton = "joystick {0} button 2";

		public void SetPlayerNumber(int playerNumber)
		{
			this.PlayerNumber = playerNumber;
			leftStickHorizontalAxis = string.Format (leftStickHorizontalAxis, playerNumber);
			jumpButton = string.Format (jumpButton, playerNumber);
			cleanButton = string.Format (cleanButton, playerNumber);
		}

		void Start () 
		{
			rigidBody = GetComponent<Rigidbody2D> ();
			myCollider = GetComponent<Collider2D> ();
		}

		void FixedUpdate () 
		{

			isGrounded = IsGrounded ();

			// Jump

			float lr = Input.GetAxis (leftStickHorizontalAxis);
			int move = Convert.ToInt32(Input.GetKey (KeyCode.RightArrow)) - Convert.ToInt32(Input.GetKey (KeyCode.LeftArrow));	
			rigidBody.velocity = new Vector2 (lr * speed, rigidBody.velocity.y);

			// Jump

			bool jump = Input.GetKeyDown (jumpButton);

			if (jump && isGrounded)
			{
				rigidBody.velocity = new Vector2 (rigidBody.velocity.x, rigidBody.velocity.y + jumpHeight * (jump ? 1 : 0));
			}			

			// 

		}

		bool IsGrounded()
		{
			RaycastHit2D raycast = Physics2D.Raycast (transform.position, Vector2.down, myCollider.bounds.extents.y+ 0.03f);
			if (raycast.collider != null) {
				transform.parent = raycast.collider.transform;
			
			} else {
				transform.parent = null;
			}
			return raycast.collider != null;
		}

		void OnTriggerStay2D(Collider2D other)
		{
			bool clean = Input.GetKeyDown (cleanButton);

			if (clean && isGrounded)
			{
				Window window = other.GetComponent<Window> ();
				if (window != null)
				{
					window.SetCleaned (this);
				}

			}
		}

	}

}
