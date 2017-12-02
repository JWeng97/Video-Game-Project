using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundSelect : MonoBehaviour {
    public Texture backgroundTexture;
    public float guiPlacementX1;
    public float guiPlacementX2;


    public float guiPlacementY1;
    public float guiPlacementY2;
	// Use this for initialization
	private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * 0.5f, Screen.height * 0.1f), "Best of 11")){
            
            // Load the round selection screen
			GameManager.instance.numberOfRounds = 11;
            SceneManager.LoadScene(1);
        }

        if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * 0.5f, Screen.height * 0.1f), "Best of 15"))
        {
			GameManager.instance.numberOfRounds = 15;
            Application.Quit();
        }
    }
}
