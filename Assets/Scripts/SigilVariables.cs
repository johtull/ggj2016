using UnityEngine;
using System.Collections;

public class SigilVariables : MonoBehaviour {

    //ID
    public int ID;

    //GRAPHICS
    public Transform Graphic;
   // public Animator ani;
    //SKIN
    public Renderer rend;
    public Texture sig0;
    public Texture sig1;
    public Texture sig2;
    public Texture safe;

    // Use this for initialization
    void Start () {
       
       // getResources();
      //  targetGraphic = this.gameObject.transform.GetChild(0);


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
            //transform.localScale = new Vector3(.7f,1,1);
        }
        else
        {
            rend.material.mainTexture = safe;
        }
    }

    // Update is called once per frame
    void Update () {

    }
    /*
    void getResources()
    {
        blue = Resources.Load("Images/BlueTileA") as Texture;
        blue2 = Resources.Load("Images/BlueTileB") as Texture;
        red = Resources.Load("Images/RedTileA") as Texture;
        red2 = Resources.Load("Images/RedTileB") as Texture;
        green = Resources.Load("Images/GreenTileA") as Texture;
        green2 = Resources.Load("Images/GreenTileB") as Texture;
        green3 = Resources.Load("Images/GreenTileC") as Texture;
        green4 = Resources.Load("Images/GreenTileD") as Texture;

    }
*/
    void OnTriggerStay2D(Collider2D col){
        if(col.tag == "Player" && col.GetComponent<GameScript>().collecting){
            col.GetComponent<GameScript>().sigilsRemaining -= 1;
            Destroy(this.gameObject);

        }
    }
}
    
