using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

    public GameObject musicGen;
    public GameObject map;
    public bool gateOpen;
    public bool collecting;
    public int sigilsRemaining;
	// Use this for initialization
	void Start () {
        map = GameObject.Find("TileBasedMapGen");
        collecting = false;
        gateOpen = false;
        sigilsRemaining = map.GetComponent<TileMapGenerator>().sigilCount; 
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
	}
      
}
