using UnityEngine;
using System.Collections;

public class BulletCode : MonoBehaviour {

	public int timer = 0;
	public int lifespan = 100;
	public int speed = 4;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector2.up * Time.deltaTime * speed);

		timer += 1;
		if(timer >= lifespan)
			Destroy(this.gameObject);
	}

	//When Bullet collides with other objects, send a message generically and destroy itself
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.name != "BulletTest(Clone)")
			Destroy (this.gameObject);
	}



}
