using UnityEngine;
using System.Collections;

public class SolutionPanelVariables : MonoBehaviour {

    public int ID;

    public Renderer rend;
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
    public Texture safe;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Get correct image
        if(ID == 0)
        {
            rend.material.mainTexture = sig0;
        }
        else if (ID == 1)
        {
            rend.material.mainTexture = sig1;
        }
        else if (ID == 2)
        {
            rend.material.mainTexture = sig2;
        }
        else if(ID == 3)
        {
            rend.material.mainTexture = sig3;
        }
        else if (ID == 4)
        {
            rend.material.mainTexture = sig4;
        }
        else if (ID == 5)
        {
            rend.material.mainTexture = sig5;
        }
        else if(ID == 6)
        {
            rend.material.mainTexture = sig6;
        }
        else if (ID == 7)
        {
            rend.material.mainTexture = sig7;
        }
        else if (ID == 8)
        {
            rend.material.mainTexture = sig8;
        }
        else if(ID == 9)
        {
            rend.material.mainTexture = sig9;
        }
        else
        {
            rend.material.mainTexture = safe;
        }
	}
}
