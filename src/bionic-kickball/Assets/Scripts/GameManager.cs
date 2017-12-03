using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int numberOfRounds = 5;
    public int roundsPlayed = 0;

    public int p1Score = 0;
    public int p2Score = 0;
    public AudioClip[] songs;
    private AudioSource audioSource;
    


   

    // Use this for initializatio
    void Awake () {
        // Create singleton GameManager
        if (instance == null)
            instance  = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        

        audioSource = this.GetComponent<AudioSource>();
	}

    public void LoadNextLevel() {
        if (numberOfRounds <= roundsPlayed) {
            SceneManager.LoadScene(11);
            audioSource.clip = songs[0];
            audioSource.Play();
        } else {
        SceneManager.LoadScene(Random.Range(2, 11));
            FadeManager.instance.BeginFade(-1);
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
