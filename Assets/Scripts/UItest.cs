using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UItest : MonoBehaviour {

	public int timer;
	public Text timerText;
	public GameObject UI;
	public GameObject timerObject;
	public GameObject b1, b2, b3;
	public GameObject playerObj;

	// Use this for initialization
	void Start () {
		timer = 0;
		UI = this.gameObject;
		timerObject = GameObject.Find("TimeText");
		timerText = timerObject.GetComponent<Text>();
		b1 = GameObject.Find("Bt1");
		b2 = GameObject.Find("Bt2");
		b3 = GameObject.Find("Bt3");


	}
	
	// Update is called once per frame
	void Update () {
		timer += 1;
		timerText.text = "Time: " + timer;
	}

	void SpeedUp(){
		playerObj.GetComponent<TankMovement>().speed += 1;
	}
	void SpeedDown(){
		playerObj.GetComponent<TankMovement>().speed -= 1;
	}
	void TimerReset(){
		timer = 0;
	}

}
