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
    public bool gameOver = false;


	// Use this for initialization
	void Start () {
        map = GameObject.Find("TileBasedMapGen");
        collecting = false;
        gateOpen = false;
        exitOpen = false;
        sigilsRemaining = map.GetComponent<TileMapGenerator>().sigilCount; 
        deposited = sigilsRemaining;
        inventory = new Queue<int>();
        gameOver = false;
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
            health += .45f;

        //floor and cap
        if(health <= 0){
            health = 0;
            gameOver = true;
            Destroy(GameObject.Find("Record"));
            Destroy(this.gameObject.GetComponent<FourWayMovement>());
        }
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
                health += 200;
                col.gameObject.GetComponent<AltarScript>().active = true;
               
            }
        }
        if(col.gameObject.tag == "LevelEnder"){
            int diff = GameObject.Find("Record").GetComponent<ProgressTracker>().level;
            string destination = "TitleScreen";
            if(GameObject.Find("Record") == null)
                diff = 1;

            //beat level 1, go to loading screen 2
            if(diff == 1)
                destination = "LoadingScreen2";

            GameObject.Find("Record").GetComponent<ProgressTracker>().level += 1;
            Application.LoadLevel(destination);

        }
    }

    void addToInventory(int i){
        inventory.Enqueue(i);

    }
}
