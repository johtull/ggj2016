using UnityEngine;
using System.Collections;

public class FourWayMovement : MonoBehaviour {

	public float moveX;
	public float moveY;
	public float speed;


	// Use this for initialization
	void Start () {
		moveX = 0;
		moveY = 0;
		speed = 3;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.A) ){
			moveX = -1;
			//transform.Translate(-.1f,0,0);
		}
		if(Input.GetKey(KeyCode.D) ){
			moveX = +1;
		}
		if(Input.GetKey(KeyCode.W) ){
			moveY = +1;
		}
		if(Input.GetKey(KeyCode.S) ){
			moveY = -1;
		}
		if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
		{
			moveX = 0;
		}
		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
		{
			moveY = 0;
		}

	/*
		if(Input.GetKey(KeyCode.T) ){
			transform.Translate (Vector2.up * .1f);
		}
		if(Input.GetKey(KeyCode.F) ){
			transform.Translate (Vector2.left * .1f);
		}
		if(Input.GetKey(KeyCode.G) ){
			transform.Translate (Vector2.down * .1f);
		}
		if(Input.GetKey(KeyCode.H) ){
			transform.Translate (Vector2.right * .1f);
		}*/
	}

	void FixedUpdate ()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * speed, moveY * speed);
	}
}
