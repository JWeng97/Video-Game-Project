using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Texture backgroundTexture;
    public float guiPlacementX1;
    public float guiPlacementX2;
    public float guiPlacementX3;

    public float guiPlacementY1;
    public float guiPlacementY2;
    public float guiPlacementY3;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * 0.5f, Screen.height * 0.1f), "Play Game")){
            SceneManager.LoadScene(Random.Range(1, 7));
        }
        if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * 0.5f, Screen.height * 0.1f), "Options"))
        {
            print("Clicked Options (This doesn't do anything yet)");
        }
        if (GUI.Button(new Rect(Screen.width * guiPlacementX3, Screen.height * guiPlacementY3, Screen.width * 0.5f, Screen.height * 0.1f), "Quit"))
        {
            Application.Quit();
        }
    }
}
