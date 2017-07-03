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
		public float respawnTimer = 4f;

		public Color color;

		Rigidbody2D rigidBody;
		Collider2D myCollider;
		Animator animator;

		public GameObject Gondola;

		bool isCleaning;
		bool isGrounded;
		bool isStomped;

		bool shouldRespawnNow;

		public bool isDisabled
		{
			get
			{
				return isCleaning || isStomped;
			}
		}

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

			isGrounded = IsGrounded ();

			// Animation parameters
			animator.SetBool ("IsCleaning", isCleaning);
			animator.SetBool ("IsFalling", !isGrounded && rigidBody.velocity.y < -8);


				
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
			{
				animator.SetTrigger ("Jump");
				rigidBody.velocity = new Vector2 (rigidBody.velocity.x, rigidBody.velocity.y + jumpHeight * (jump ? 1 : 0));
			}

			// Stomped Color

			if (!isStomped)
			{
				GetComponent<SpriteRenderer> ().color = Color.white;
			}

			if (shouldRespawnNow) {
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				Vector3 gondolaBtmPos = Gondola.transform.GetChild (1).transform.position;
				gondolaBtmPos.y = 6;
				transform.position = gondolaBtmPos;

				shouldRespawnNow = false;
			}

		}

		bool IsGrounded()
		{
			RaycastHit2D raycast = Physics2D.Raycast (transform.position, Vector2.down, myCollider.bounds.extents.y+ 0.05f);
			if (raycast.collider != null && raycast.collider.tag == "Standable") {
				transform.parent = raycast.collider.transform;
			
			} else {
				transform.parent = null;
			}
			return raycast.collider != null;
		}

		void OnTriggerStay2D(Collider2D other)
		{
			if( other.IsTouching(GetComponent<CapsuleCollider2D>())){
				
				bool clean = Input.GetKeyDown (cleanButton);

				if (clean && isGrounded && !isCleaning )
				{
					Window window = other.GetComponent<Window> ();
					if (window != null)
					{
						isCleaning = true;

						Timer timer = new Timer ();
						timer.Interval = cleanTime * 1000;
						timer.Elapsed += (sender, e) =>
						{
							timer.Stop();
							isCleaning = false;
						};
						timer.Start ();
						window.SetCleaned (this);
					}

				}
			}
		}

		void Stomped()
		{
			isStomped = true;
			GetComponent<SpriteRenderer> ().color = Color.red;

			Timer timer = new Timer ();
			timer.Interval = stompDuration * 1000;
			timer.Elapsed += (sender, e) =>
			{
				timer.Stop ();
				isStomped = false;
			};
			timer.Start ();
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.transform.tag == "Player" && other.transform.GetComponent<Rigidbody2D> ().velocity.y < -0.5f) 
			{
				Stomped ();
			}
		}


		void OnBecameInvisible(){
			// Teleport to above gondola
			Timer timer = new Timer ();
			timer.Interval = respawnTimer * 1000;
			timer.Elapsed += (sender, e) =>
			{
				timer.Stop ();
				shouldRespawnNow = true;
			};
			timer.Start ();
		
		}
	

	}

}
