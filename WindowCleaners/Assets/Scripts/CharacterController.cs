using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

namespace WindowCleaner
{

	public class CharacterController : MonoBehaviour {

		public float speed = 10f;
		public float jumpHeight = 10f;
		public float cleanTime = 1f;
		public float stompDuration = 1f;

		public Color color;

		Rigidbody2D rigidBody;
		Collider2D myCollider;
		Animator animator;

		bool isCleaning;
		bool isGrounded;
		bool isStomped;

		float stompTimer;

		public int cleanedWindows = 0;

		public int PlayerNumber{ get; private set; }

		string leftStickHorizontalAxis = "joystick {0} Left Horizontal";
		string jumpButton = "joystick {0} button 1";
		string cleanButton = "joystick {0} button 2";

		Vector3 initialScale;

		public void SetPlayerNumber(int playerNumber)
		{
			this.PlayerNumber = playerNumber;
			leftStickHorizontalAxis = string.Format (leftStickHorizontalAxis, playerNumber);
			jumpButton = string.Format (jumpButton, playerNumber);
			cleanButton = string.Format (cleanButton, playerNumber);
		}

		void Start () 
		{
			initialScale = transform.localScale;

			rigidBody = GetComponent<Rigidbody2D> ();
			myCollider = GetComponent<Collider2D> ();
			animator = GetComponent<Animator> ();
		}

		void FixedUpdate () 
		{
			animator.SetBool ("IsCleaning", isCleaning);
			isGrounded = IsGrounded ();

			if (isStomped) {
				stompTimer -= Time.deltaTime;
				if (stompTimer <= 0) {
					isStomped = false;
					GetComponent<SpriteRenderer> ().color = Color.white;
				}
			}
			else{
				// Jump

			if (!isCleaning)
			{
				float lr = Input.GetAxis (leftStickHorizontalAxis);
				int move = Convert.ToInt32 (Input.GetKey (KeyCode.RightArrow)) - Convert.ToInt32 (Input.GetKey (KeyCode.LeftArrow));	
				rigidBody.velocity = new Vector2 (lr * speed, rigidBody.velocity.y);

				animator.SetBool ("IsRunning", Math.Abs (lr) > 0);

				Vector3 iScale = transform.localScale;
				float xScale = Math.Abs (iScale.x);
				transform.localScale = new Vector3 (lr >= 0 ? xScale : -xScale, iScale.y, iScale.z);
			}

				// Jump

				bool jump = Input.GetKeyDown (jumpButton);

			if (jump && isGrounded && !isCleaning)
					rigidBody.velocity = new Vector2 (rigidBody.velocity.x, rigidBody.velocity.y + jumpHeight * (jump ? 1 : 0));
				}	
			}



		}

		bool IsGrounded()
		{
			RaycastHit2D raycast = Physics2D.Raycast (transform.position, Vector2.down, myCollider.bounds.extents.y+ 0.03f);
			if (raycast.collider != null && raycast.collider.tag == "Standable") {
				transform.parent = raycast.collider.transform;
			
			} else {
				transform.parent = null;
			}
			return raycast.collider != null;
		}

		void OnTriggerStay2D(Collider2D other)
		{
			bool clean = Input.GetKeyDown (cleanButton);

			if (clean && isGrounded && !isCleaning)
			{
				Window window = other.GetComponent<Window> ();
				if (window != null)
				{
					isCleaning = true;
					Timer timer = new Timer ();
					timer.Interval = cleanTime;
					timer.Enabled = true;
					timer.Elapsed += (sender, e) => CleaningComplete(sender, e, window);
					window.SetCleaned (this);
				}

			}
		}

		private void Stomped(){
			isStomped = true;
			stompTimer = stompDuration;
			GetComponent<SpriteRenderer> ().color = Color.red;
		}

		void OnTriggerEnter2D(Collider2D other){
			if (other.transform.tag == "Player") {
				if (other.transform.GetComponent<Rigidbody2D> ().velocity.y < 0) {
					Stomped ();
				}
			
			}
		}

		void CleaningComplete(object sender, ElapsedEventArgs e, Window window)
		{
			isCleaning = false;

		}

	}

}
