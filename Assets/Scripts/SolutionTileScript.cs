using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolutionTileScript : MonoBehaviour {

    public Transform tile;
    public int duration = 100;

    List<int> solution;
    //int[] solu = new int[];


	// Use this for initialization
	void Start () {
        solution = new List<int>();

	}
	
	// Update is called once per frame
	void Update () {
        duration -=1;
        if(duration<=0)
            Destroy(this.gameObject);
	}

   public void setSolution(List<int> sol){


        foreach(int i in sol)
        {
            Vector3 pos;
            if(i < 5)
                pos = new Vector3((i*1.5f)-3,-5,-8);
            else
                pos = new Vector3((i*1.5f)-10.5f,-7,-8);

            Transform newTile = Instantiate(tile, pos, Quaternion.identity) as Transform;
            newTile.localScale = Vector3.one * 1.2f;
            newTile.gameObject.GetComponent<SolutionPanelVariables>().ID = sol[i];
            newTile.parent = this.transform;

        }
           

    }
}
