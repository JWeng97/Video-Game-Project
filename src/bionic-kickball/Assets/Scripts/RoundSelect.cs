using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundSelect : MonoBehaviour {
	public Text numberOfRounds;
    public Texture backgroundTexture;
    public float guiPlacementX1;
    public float guiPlacementX2;
	public float guiPlacementX3;

    public float guiPlacementY1;
    public float guiPlacementY2;
	public float guiPlacementY3;
	// Use this for initialization
	private void OnGUI()
    {

		if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * 0.1f, Screen.height * 0.1f), "+1")) {
			GameManager.instance.numberOfRounds++;
		}

		if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY1, Screen.width * 0.1f, Screen.height * 0.1f), "-1")) {
			if (GameManager.instance.numberOfRounds > 1)
				GameManager.instance.numberOfRounds--;
		}
	
        if (GUI.Button(new Rect(Screen.width * guiPlacementX3, Screen.height * guiPlacementY2, Screen.width * 0.5f, Screen.height * 0.1f), "Play"))
        {
			GameManager.instance.LoadNextLevel(); 
        }

		 if (GUI.Button(new Rect(Screen.width * guiPlacementX3, Screen.height * guiPlacementY3, Screen.width * 0.5f, Screen.height * 0.1f), "Back"))
        {
            SceneManager.LoadScene(0);
			GameManager.instance.WipeScore();
        }
    }

	void Update() {
		numberOfRounds.text = GameManager.instance.numberOfRounds.ToString();
	}
}
