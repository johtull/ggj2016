using UnityEngine;
using System.Collections;

public class AltarScript : MonoBehaviour {

    //Variables
    public int ID;
    public int offering;
    public bool active;
    public int correct;
    public bool tallied;

    //GRAPHICS
    public Transform Graphic;
    // public Animator ani;
    //SKIN
    public Renderer rend;
    public Texture ActiveGraphic;

    // Use this for initialization
    void Start () {
        active = false;
        correct = 0;
        tallied = false;
    }

    // Update is called once per frame
    void Update () {
        if(active){
        rend.material.mainTexture = ActiveGraphic;
            if(!tallied){
                GameObject.Find("Record").GetComponent<ProgressTracker>().score += correct;
                tallied = true;
            }

        }

        if(offering == ID)
            correct = 0;
        else
            correct = 1;

    }

}
