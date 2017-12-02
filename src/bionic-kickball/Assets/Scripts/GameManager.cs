using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Texture2D fadeOutTexture;
    public float fadeSpeed;

    private int drawDepth = -1000;
    private float alpha = 1.0f; 
    private int fadeDir = -1;          //-1 = fade in, 1 = fade out

    public int numberOfRounds = 0;
    public int roundsPlayed = 0;

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        //force/clamp value to zero and one
        alpha = Mathf.Clamp01(alpha);
        //set color of GUI. Colors should ideally remain the same, and only alpha is changed
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b,alpha);
        GUI.depth = drawDepth; //render on Top
        GUI.DrawTexture ( new Rect(0, 0, Screen.width, Screen.height),fadeOutTexture); //Texture fits entire screen

    }


    public float BeginFade ( int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        //alpha = 1
        BeginFade(-1);
    }

    // Use this for initialization
    void Awake () {
        // Create singleton GameManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        fadeOutTexture = new Texture2D(1,1, TextureFormat.ARGB32, false);
        fadeOutTexture.SetPixel(0, 0, Color.black);
        fadeOutTexture.Apply();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
