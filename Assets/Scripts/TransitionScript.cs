using UnityEngine;
using System.Collections;

public class TransitionScript : MonoBehaviour {

    // Use this for initialization
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel("MainTestRoom");
        }

    }
}
