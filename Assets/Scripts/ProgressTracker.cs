using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour {


    public int score = 0;
    public int level = 1;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}
