using UnityEngine;
using System.Collections;

public class AutoTransition : MonoBehaviour {
    int times = 300;
	// Use this for initialization
	void Start () {
        times = 300;
    }
	
	// Update is called once per frame
	void Update () {
        times = times - 1;
        if (times <= 0)
        Application.LoadLevel("TitleScreen");
    }
}
