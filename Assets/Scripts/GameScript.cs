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
    public float health = 1000f;
    public bool dark = true;
    public GameObject light;


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
        if(Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space) ){
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

        //take damage in dark, heal in light
        if(dark)
            health -= .35f;
        else
            health += .35f;

        //floor and cap
        if(health <= 0)
            health = 0;
        if(health >= 1000)
            health = 1000;

        //scale lighting with health
        light.gameObject.GetComponent<Light>().intensity = (health/1000f) *4;

        //debug health
        print("Health:" + health);
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
