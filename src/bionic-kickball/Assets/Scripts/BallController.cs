using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	public GameObject ball;
	public GameObject[] balls;
	private bool grounded;
	private CircleCollider2D _circleCollider;
	private float distToGround;
	private GameObject redTrail;
	private GameObject blueTrail;
	
	private GameObject playerWhoKickedLast;
	private bool countdownRunning = false;

	// Use this for initialization
	void Awake() {
		_circleCollider = this.GetComponent<CircleCollider2D>();
		distToGround = _circleCollider.bounds.extents.y;
		redTrail = transform.Find("RedTrail").gameObject;
		blueTrail = transform.Find("BlueTrail").gameObject;
		balls = GameObject.FindGameObjectsWithTag("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		if (playerWhoKickedLast != null){
			if (GetPlayerWhoKickedLast().name == "Player1") {
				blueTrail.SetActive(true);
				redTrail.SetActive(false);
			} else {
				redTrail.SetActive(true);
				blueTrail.SetActive(false);
			}
		}
		/*if (!countdownRunning) {
			StartCoroutine(DuplicateCountdown());
		}*/
	}

	IEnumerator DuplicateCountdown() {
		countdownRunning = true;
		yield return new WaitForSeconds(30f);
		DuplicateBalls();
		countdownRunning = false;
	}

	void DuplicateBalls() {
		foreach (GameObject b in balls) {
			Vector3 newPosition = new Vector3(b.transform.position.x + 1f, b.transform.position.y, b.transform.position.z);
			Instantiate(b, newPosition, b.transform.rotation); 
		}
	}

	public void SetPlayerWhoKickedLast(GameObject player) {
		playerWhoKickedLast = player;
	}
	
	public GameObject GetPlayerWhoKickedLast() {
		return playerWhoKickedLast;
	}

	public bool isGrounded() {
		Debug.Log(Physics.Raycast(transform.position, Vector3.down, distToGround + 0.01f));
		return true;
	}
}
