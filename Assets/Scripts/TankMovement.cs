using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour {

	public float speed;
	public float rotSpeed;
	public bool momentum;
	public AudioClip testnoise;
	AudioSource audio;

	//bullet object
	public GameObject bulletObj;

	// Use this for initialization
	void Start () {
		speed = 3;
		rotSpeed = 2;
		momentum = false;
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.A) ){
			transform.Rotate(0,0,rotSpeed);
		}
		if(Input.GetKey(KeyCode.D) ){
			transform.Rotate(0,0,-rotSpeed);
		}
		if(Input.GetKey(KeyCode.W)&&!momentum){
			transform.Translate(Vector2.up * Time.deltaTime * speed);
		}
		if(Input.GetKey(KeyCode.S)&&!momentum){
			transform.Translate(Vector2.up * Time.deltaTime * -speed);
		}
		if(Input.GetKey(KeyCode.E) ){
			shoot();
		}

	}

	void FixedUpdate ()
	{
	//	GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
	}

	void shoot()
	{
		Quaternion newRot = this.gameObject.transform.rotation;
		Vector3 newDir = this.gameObject.transform.up;
		Vector2 newPos = this.gameObject.transform.position + newDir * 1;

		audio.PlayOneShot(testnoise);
		Instantiate(bulletObj, newPos, newRot);

	}

}
