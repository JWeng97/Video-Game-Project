using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;


public class PlayerController: MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	// The ball object 
	public GameObject ball;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private SpriteRenderer _spriteRenderer;
	private Vector3 _velocity;
	private BallController _ballController;
	private float kickPower = 0;
	// TEST value for how much power a kick can accumulate
	public float kickPowerLimit = 3f;


	// TEST value for when ball can be kicked
	public float acceptablePToBDistance = 2f;
	private float playerToBallDistance; 

	// TEST count how many times a player has been hit
	public Text playerDeaths;
	private int deathCount = 0;

	//Per-Player Settings
	/* Player id# */
	public int playerID = 1;
	/* movement controls */
	public KeyCode leftKey = KeyCode.LeftArrow;
	public KeyCode rightKey = KeyCode.RightArrow;
	public KeyCode upKey = KeyCode.UpArrow;
	public KeyCode downKey = KeyCode.DownArrow;
	/* kicking controls */
	public KeyCode kickKey = KeyCode.K;
	public KeyCode slideKey = KeyCode.L;

	private bool hasLandedSinceDivekick;
	private bool isDivekicking = false;
	private bool isSliding = false;
	public Vector3 diveKickForce = new Vector3(150f, -150f, 0);
	private bool SkipInputControls = false;
	private bool facingRight = false;
	private bool slideOnCooldown = false;
	private bool rotationOnCooldown = false;
	private Quaternion startingRotation;


	void Awake()
	{
		startingRotation = transform.rotation;
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_ballController = ball.GetComponent<BallController>();
	
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		SetPlayerDeathText();
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		if (hit.collider.tag == "HeadCollider") {
			StartCoroutine(HitByBall());
		}

		// to kick a ball encountered by a player
		if (hit.collider.tag == "Ball") {
			KickBall();
		}


		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}

	void onTriggerEnterEvent( Collider2D col )
	{
		if (col.tag == "Ball" && _ballController.GetPlayerWhoKickedLast() != this.gameObject) {
			Debug.Log("onTriggerEnterEvent: PLAYER HIT! " + col.gameObject.name );
			// Eventually need to change this to accuont 
			StartCoroutine(HitByBall());
		}
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion

	// Triggered when player collides with the ball near the feet
	
	void SetPlayerDeathText() {
		playerDeaths.text = "Player " + playerID + " Deaths: " + deathCount;
	}
	void KickBall() 
	{	
		_ballController.SetPlayerWhoKickedLast(this.gameObject);
		kickPower = 3;
		Vector3 angle = this.transform.forward;
		int direction = (this._velocity.x > 0) ? 1 : -1;
		Vector3 playerVelocity = this._velocity;
		ball.GetComponent<Rigidbody2D>().velocity = new Vector3( direction * (angle.x * kickPower + Random.Range(5,10)), angle.y * kickPower + Random.Range(1,10)) + playerVelocity;
	}

	// the animation for when a player has been hit by a ball
	IEnumerator HitByBall() 
	{
		deathCount++;
		SetPlayerDeathText();
		this.GetComponent<CircleCollider2D>().enabled = false;
		for (int i = 0; i < 10; i++) {
			_spriteRenderer.enabled = false;
			yield return new WaitForSeconds(0.1f);
			_spriteRenderer.enabled = true;
			yield return new WaitForSeconds(0.1f);
		}
		this.GetComponent<CircleCollider2D>().enabled = true;
	}

	void DiveKick(float facingDirection) {
		isDivekicking = true;
		_animator.Play("p1_slide");
		Quaternion originalRotation = transform.rotation;	
		transform.rotation = Quaternion.AngleAxis(-45 * facingDirection, Vector3.forward);
	}

	void Slide() {
		isSliding = true;
	}

	// Adds gravity to a move
	void AddGravity() {
		_velocity.y += gravity * Time.deltaTime;
	}

	IEnumerator SlideCooldown() {
		slideOnCooldown = true;
		yield return new WaitForSeconds(1f);
		slideOnCooldown = false;
	}
	
	IEnumerator RotationCooldown() {
		rotationOnCooldown = true;
		yield return new WaitForSeconds(.1f);
		rotationOnCooldown = false;
	}

	void SlideOrRun() {
		if( _controller.isGrounded ) {
			if (Input.GetKey(slideKey) && !slideOnCooldown) {
				_animator.Play("p1_slide");
				StartCoroutine(SlideCooldown());
			} else {
				_animator.Play("p1_run");
			} 
		// if not grounded then attempting to Divekick
		} else if (Input.GetKey(slideKey) && hasLandedSinceDivekick) {
			print("trying to divekick");
			//	DiveKick(normalizedHorizontalSpeed);
		}
	}

	void Update()
	{
		if( _controller.isGrounded ) {
			if (isDivekicking) {
				isDivekicking = false;
				SkipInputControls = false;
				hasLandedSinceDivekick = true;
				if (!rotationOnCooldown){
					transform.rotation = startingRotation;
					StartCoroutine(RotationCooldown());
				}
			}
			_velocity.y = 0;
		} else if (this.isDivekicking) {
			// want to continue divekick and not allow other movement
			print(isDivekicking);
			AddGravity();
			SkipInputControls = true;
		}

		if (!SkipInputControls) {
			// Move right and set correct animation
			if(!_controller.isGrounded && Input.GetKey(slideKey)) {
				int dir = (facingRight) ? 1 : -1;
				DiveKick(dir);
			}
			else if( Input.GetKey( rightKey ) )
			{
				normalizedHorizontalSpeed = 1;
				facingRight = true;
				if( transform.localScale.x < 0f ) {
					transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
				}
				SlideOrRun();	
			}

			// Move left and set correct animation
			else if( Input.GetKey( leftKey ) )
			{
				normalizedHorizontalSpeed = -1;
				facingRight = false;
				if( transform.localScale.x > 0f ) {
					transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
				}
				SlideOrRun();

			} else if (Input.GetKey(slideKey)) {
				//Just Sliding not divekicking, apply different force
				_animator.Play("p1_slide");
				print("sliding");
			}
			else
			{
				normalizedHorizontalSpeed = 0;

				if( _controller.isGrounded )
					_animator.Play("p1_idle");
			
			}
		}
		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetKeyDown( upKey ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play("p1_jump_up");
		}

		//TODO: Should we use SmoothDamp?
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		if (isDivekicking) {
			float horizontalForce = (facingRight) ?  diveKickForce.x  : -1 * diveKickForce.x;
			_velocity.x = Mathf.Lerp(_velocity.x, horizontalForce, Time.deltaTime * smoothedMovementFactor );
			_velocity.y += diveKickForce.y;
		}
		// apply gravity before moving
		AddGravity();

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
/*		if( _controller.isGrounded && Input.GetKey( downKey ) )
		{
			Debug.Log("Trying to go through one way platform");
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}*/



		_controller.recalculateDistanceBetweenRays();
		_controller.move( _velocity * Time.deltaTime );


		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

}