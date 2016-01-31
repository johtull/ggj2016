using UnityEngine;
using System.Collections;

public class Flame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D col){
        if(col.tag == "Player"){
            col.GetComponent<GameScript>().dark = false;
          //  col.SendMessage("addToInventory",ID);
          //  Destroy(this.gameObject);

        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.tag == "Player"){
            col.GetComponent<GameScript>().dark = true;
            //  col.SendMessage("addToInventory",ID);
            //  Destroy(this.gameObject);

        }
    }
}
