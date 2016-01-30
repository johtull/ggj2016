using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {

    public GameObject musicGen;
    public GameObject map;
    public bool gateOpen;
    public bool exitOpen;
    public bool collecting;
    public int sigilsRemaining;
    public Queue<int> inventory;
    public int deposited;
	// Use this for initialization
	void Start () {
        map = GameObject.Find("TileBasedMapGen");
        collecting = false;
        gateOpen = false;
        exitOpen = false;
        sigilsRemaining = map.GetComponent<TileMapGenerator>().sigilCount; 
        deposited = sigilsRemaining;
        inventory = new Queue<int>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.E) ){
            collecting = true;
        }
        else
            collecting = false;

        if(sigilsRemaining <= 0 && !gateOpen){
            GameObject gate = GameObject.FindGameObjectWithTag("ChamberGate");
            Destroy(gate.gameObject);
            gateOpen = true;
        }
        if(deposited <= 0 && !exitOpen){
            GameObject exit= GameObject.FindGameObjectWithTag("ExitGate");
            Destroy(exit.gameObject);
            exitOpen = true;
        }
	}
      

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Altar" && collecting){
            if(!col.gameObject.GetComponent<AltarScript>().active){
               col.gameObject.GetComponent<AltarScript>().offering = inventory.Dequeue();
                deposited -= 1;
                col.gameObject.GetComponent<AltarScript>().active = true;
               
            }
        }
    }

    void addToInventory(int i){
        inventory.Enqueue(i);

    }
}
