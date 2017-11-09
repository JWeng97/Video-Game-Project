using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	public GameObject ball;
	private bool grounded;
	private CircleCollider2D _circleCollider;
	private float distToGround;
	private GameObject redTrail;
	private GameObject blueTrail;
	
	private GameObject playerWhoKickedLast;

	// Use this for initialization
	void Awake() {
		_circleCollider = this.GetComponent<CircleCollider2D>();
		distToGround = _circleCollider.bounds.extents.y;
		redTrail = transform.Find("RedTrail").gameObject;
		blueTrail = transform.Find("BlueTrail").gameObject;
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
