using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Texture2D fadeOutTexture;
    public float fadeSpeed;

    private int drawDepth = -1000;
    private float alpha = 1.0f; 
    private int fadeDir = -1;          //-1 = fade in, 1 = fade out

    public int numberOfRounds = 5;
    public int roundsPlayed = 0;

    public int p1Score = 0;
    public int p2Score = 0;
    public AudioClip[] songs;
    private AudioSource audioSource;
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

    // Use this for initializatio
    void Awake () {
        // Create singleton GameManager
        if (instance == null)
            instance  = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        fadeOutTexture = new Texture2D(1,1, TextureFormat.ARGB32, false);
        fadeOutTexture.SetPixel(0, 0, Color.black);
        fadeOutTexture.Apply();

        audioSource = this.GetComponent<AudioSource>();
	}

    public void LoadNextLevel() {
        if (numberOfRounds <= roundsPlayed) {
            SceneManager.LoadScene(11);
            audioSource.clip = songs[0];
            audioSource.Play();
        } else {
        SceneManager.LoadScene(Random.Range(2, 11));
        if (audioSource.clip != songs[1]) {
            audioSource.clip = songs[1];
            audioSource.Play();
        }
        }
		roundsPlayed++;
    }

    public void WipeScore() {
        p1Score = 0;
        p2Score = 0;
        roundsPlayed = 0;
        numberOfRounds = 5;
    }

	// Update is called once per frame
	void Update () {

	}
}
