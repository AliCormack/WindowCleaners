using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour {

	public float speed = 10f;
	public float jumpHeight = 10f;

	Rigidbody2D rigidBody;
	Collider2D collider;

	void Start () 
	{
		rigidBody = GetComponent<Rigidbody2D> ();
		collider = GetComponent<Collider2D> ();
	}

	void FixedUpdate () 
	{
		// TODO change to floating point controller input
		int move = Convert.ToInt32(Input.GetKey (KeyCode.D)) - Convert.ToInt32(Input.GetKey (KeyCode.A));	

		rigidBody.velocity = new Vector2 (move * speed, rigidBody.velocity.y);

		int jump = Convert.ToInt32(Input.GetKeyDown (KeyCode.W));

		if (jump > 0 && IsGrounded ())
		{
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, rigidBody.velocity.y + jumpHeight * jump);
		}

	}

	bool IsGrounded()
	{
		RaycastHit2D raycast = Physics2D.Raycast (transform.position, Vector2.down, collider.bounds.extents.y+ 0.03f);
		Debug.Log (raycast.collider);
		return raycast.collider != null;
	}

}
