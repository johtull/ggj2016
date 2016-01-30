using UnityEngine;
using System.Collections;

public class MusicShift : MonoBehaviour {

    public GameObject musicGen;
	// Use this for initialization
	void Start () {
        musicGen = GameObject.Find("Music");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col){
            musicGen.GetComponent<AudioSource>().volume = 1f;
        Destroy(this.gameObject);
        print("JIMMY");
    }

}
