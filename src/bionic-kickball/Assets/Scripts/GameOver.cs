using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour {

    public Texture backgroundTexture;
    public float guiPlacementX1;
    public float guiPlacementX2;


    public float guiPlacementY1;
    public float guiPlacementY2;

	public Text p1Text;

	public Text p2Text;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * 0.3f, Screen.height * 0.1f), "Back to Main Menu")){
            
            // Load the round selection screen
            SceneManager.LoadScene(0);
			GameManager.instance.WipeScore();
        }

        if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * 0.3f, Screen.height * 0.1f), "Quit"))
        {
            Application.Quit();
        }
    }

	void Start() {
		p1Text.text = GameManager.instance.p1Score.ToString();
		p2Text.text = GameManager.instance.p2Score.ToString();
	}
}
