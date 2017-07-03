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
		Collider2D collider;

		bool cleaning;

		public int cleanedWindows = 0;

		int playerNumber;
		string horizontalAxis = "joystick {0} Horizontal";
		string jumpButton = "joystick {0} button 1";
		string cleanButton = "joystick {0} button 2";

		public void SetPlayerNumber(int playerNumber)
		{
			this.playerNumber = playerNumber;
			horizontalAxis = string.Format (horizontalAxis, playerNumber);
			jumpButton = string.Format (jumpButton, playerNumber);
			cleanButton = string.Format (cleanButton, playerNumber);
		}

		void Start () 
		{
			rigidBody = GetComponent<Rigidbody2D> ();
			collider = GetComponent<Collider2D> ();
		}

		void FixedUpdate () 
		{
			// TODO change to floating point controller input

			float lr = Input.GetAxis (horizontalAxis);


			int move = Convert.ToInt32(Input.GetKey (KeyCode.RightArrow)) - Convert.ToInt32(Input.GetKey (KeyCode.LeftArrow));	

			rigidBody.velocity = new Vector2 (lr * speed, rigidBody.velocity.y);

			bool jump = Input.GetKeyDown (jumpButton);

			if (jump && IsGrounded ())
			{
				rigidBody.velocity = new Vector2 (rigidBody.velocity.x, rigidBody.velocity.y + jumpHeight * (jump ? 1 : 0));
			}
				

		}

		bool IsGrounded()
		{
			RaycastHit2D raycast = Physics2D.Raycast (transform.position, Vector2.down, collider.bounds.extents.y+ 0.03f);
			return raycast.collider != null;
		}

		void OnTriggerStay2D(Collider2D other)
		{
			bool clean = Input.GetKeyDown (cleanButton);

			if (clean && IsGrounded ())
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
