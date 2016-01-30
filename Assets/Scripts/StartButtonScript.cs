using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour {

	// Use this for initialization
	public void ChangeSceneStart (string sceneChange)
    {
        Application.LoadLevel(sceneChange);
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
