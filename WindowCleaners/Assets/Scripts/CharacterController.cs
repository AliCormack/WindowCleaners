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

		void Start () 
		{
			rigidBody = GetComponent<Rigidbody2D> ();
			collider = GetComponent<Collider2D> ();
		}

		void FixedUpdate () 
		{
			// TODO change to floating point controller input
			int move = Convert.ToInt32(Input.GetKey (KeyCode.RightArrow)) - Convert.ToInt32(Input.GetKey (KeyCode.LeftArrow));	

			rigidBody.velocity = new Vector2 (move * speed, rigidBody.velocity.y);

			int jump = Convert.ToInt32(Input.GetKeyDown (KeyCode.Space));

			if (jump > 0 && IsGrounded ())
			{
				rigidBody.velocity = new Vector2 (rigidBody.velocity.x, rigidBody.velocity.y + jumpHeight * jump);
			}
				

		}

		bool IsGrounded()
		{
			RaycastHit2D raycast = Physics2D.Raycast (transform.position, Vector2.down, collider.bounds.extents.y+ 0.03f);
			return raycast.collider != null;
		}

		void OnTriggerStay2D(Collider2D other)
		{
			int clean = Convert.ToInt32(Input.GetKeyDown (KeyCode.S));

			if (clean > 0 && IsGrounded ())
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
