using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GondolaScript : MonoBehaviour {

	public GameObject GondolaTop;

	public GameObject leftCable;
	public GameObject rightCable;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetKey(KeyCode.W)){
			transform.Translate (new Vector2 (0, 0.1f));
		}
		if(Input.GetKey(KeyCode.S)){
			transform.Translate (new Vector2 (0, -0.1f));
		}

		if(Input.GetKey(KeyCode.A)){
			transform.Translate (new Vector2 (-0.1f, 0));
			GondolaTop.transform.Translate (new Vector2 (-0.1f, 0));
		}
		if(Input.GetKey(KeyCode.D)){
			transform.Translate (new Vector2 (0.1f, 0));
			GondolaTop.transform.Translate (new Vector2 (0.1f, 0));
		}

		UpdateCable (leftCable, new Vector3(-1,0,0));
		UpdateCable (rightCable, new Vector3(1,0,0));
	}

	void UpdateCable(GameObject cable, Vector3 offset)
	{
		Vector3 direction = transform.position - GondolaTop.transform.position;
		cable.transform.position = transform.position + offset - direction / 2;

		cable.transform.rotation = Quaternion.LookRotation(direction);
		cable.transform.localScale = new Vector3(cable.transform.localScale.x,cable.transform.localScale.y,direction.magnitude);
	}


	private void OnCollisionEnter(Collision c)
	{
		if(c.transform.name == "Player")
		{
			c.transform.parent = transform;
		}
	}

	private void OnCollisionLeave(Collision c)
	{
		if(c.transform.name == "Player")
		{
			c.transform.parent = null;
		}
	}
}
