using UnityEngine;
using System.Collections;

public class BasicEnemyAI : MonoBehaviour {

	public float moveX;
	public float moveY;
	public float speed;
	public GameObject target;
	public  Vector3 targPos;

	// Use this for initialization
	void Start () {
		moveX = 0;
		moveY = 0;
		speed = 3;
		target = GameObject.Find("player");
		targPos = target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		targPos = target.transform.position;

		//X check
		if(targPos.x > transform.position.x)
			moveX = +1;
		else if(targPos.x < transform.position.x)
			moveX = -1;
		else
			moveX = 0;

		//Y check
		if(targPos.y > transform.position.y)
			moveY = +1;
		else if(targPos.y < transform.position.y)
			moveY = -1;
		else
			moveY = 0;
	}

	void FixedUpdate ()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * speed, moveY * speed);
	}
}
