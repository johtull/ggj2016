using UnityEngine;
using System.Collections;

public class AltarScript : MonoBehaviour {

    //Variables
    public int ID;
    public int offering;
    public bool active;
    public int correct;
    public bool tallied;
    //public Transform targetGraphic;

    //GRAPHICS
    public Transform Graphic;
    // public Animator ani;
    //SKIN
    public Renderer rend;
    public Texture ActiveGraphic;

    public Texture sig0;
    public Texture sig1;
    public Texture sig2;
    public Texture sig3;
    public Texture sig4;
    public Texture sig5;
    public Texture sig6;
    public Texture sig7;
    public Texture sig8;
    public Texture sig9;
   

    // Use this for initialization
    void Start () {
        active = false;
        correct = 0;
        tallied = false;
        Graphic = this.gameObject.transform.GetChild(0);
        Graphic.GetComponent<Renderer>().material.mainTexture = ActiveGraphic;
    }

    // Update is called once per frame
    void Update () {
        if(active){
        rend.material.mainTexture = ActiveGraphic;
           // Graphic.gameObject.renderer.material
            if(!tallied){
                GameObject.Find("Record").GetComponent<ProgressTracker>().score += correct;

                Texture newTex = ActiveGraphic;
                if(offering == 0)
                    newTex = sig0;
                else if(offering == 1)
                    newTex = sig1;
                else if(offering == 2)
                    newTex = sig2;
                else if(offering == 3)
                    newTex = sig3;
                else if(offering == 4)
                    newTex = sig4;
                else if(offering == 5)
                    newTex = sig5;
                else if(offering == 6)
                    newTex = sig6;
                else if(offering == 7)
                    newTex = sig7;
                else if(offering == 8)
                    newTex = sig8;
                else if(offering == 9)
                    newTex = sig9;


                    Graphic.GetComponent<Renderer>().material.mainTexture = newTex;


                tallied = true;
            }

        }

        if(offering == ID)
            correct = 0;
        else
            correct = 1;

    }

}
