using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour {
    public Texture2D fadeOutTexture;
    public float fadeSpeed;
    public static FadeManager instance = null;
    private int drawDepth = -5;
    private float alpha = 1.0f;
    private int fadeDir = -1;          //-1 = fade in, 1 = fade out
                                       // Use this for initialization
    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        //force/clamp value to zero and one
        alpha = Mathf.Clamp01(alpha);
        //set color of GUI. Colors should ideally remain the same, and only alpha is changed
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth; //render on Top
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture); //Texture fits entire screen

    }
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded(int level)
    {
        
        if (level != 1)
        {
            alpha = 1.0f;
            BeginFade(-1);
        }
    }
    void Awake()
    {
        // Create singleton GameManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        fadeOutTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        fadeOutTexture.SetPixel(0, 0, Color.black);
        fadeOutTexture.Apply();


    }
}
