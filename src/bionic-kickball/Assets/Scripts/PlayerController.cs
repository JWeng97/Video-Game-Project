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

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_ballController = ball.GetComponent<BallController>();
	
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		playerDeaths.text = "Player 1 Deaths: " + deathCount;
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}

	void onTriggerEnterEvent( Collider2D col )
	{
		if (col.tag == "Ball") {
			Debug.Log( "onTriggerEnterEvent: PLAYER HIT! " + col.gameObject.name );
			// Eventually need to change this to accuont 
			StartCoroutine(HitByBall());
		}
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion

	//Fires when space is held down and the player is near the ball
	void KickBall() 
	{
		Vector3 angle = this.transform.forward;
		int direction = (this._velocity.x > 0) ? 1 : -1;
		ball.GetComponent<Rigidbody2D>().velocity = new Vector3( direction * (angle.x * kickPower + Random.Range(5,10)), angle.y * kickPower + Random.Range(1,10));
	}

	// the animation for when a player has been hit by a ball
	IEnumerator HitByBall() 
	{
		deathCount++;
		playerDeaths.text = "Player 1 Deaths: " + deathCount;
		this.GetComponent<CircleCollider2D>().enabled = false;
		for (int i = 0; i < 10; i++) {
			_spriteRenderer.enabled = false;
			yield return new WaitForSeconds(0.1f);
			_spriteRenderer.enabled = true;
			yield return new WaitForSeconds(0.1f);
		}
		this.GetComponent<CircleCollider2D>().enabled = true;
	}

	void Update()
	{
		if( _controller.isGrounded )
			_velocity.y = 0;

		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "PlayerRun" ) );
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "PlayerRun" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "PlayerIdle" ) );
		}
		
		// Gather power to kick the ball w/ space
		if (Input.GetKey(KeyCode.Space) && kickPower < kickPowerLimit) {
			kickPower += Time.deltaTime;
			Debug.Log("increasing kick power: " + kickPower);
		} 
		if (Input.GetKeyUp(KeyCode.Space)) {
			// if released space, check to make sure ball is close enough to the player
			playerToBallDistance = Vector3.Distance(ball.transform.position, this.transform.position);
			if (kickPower > 0  && playerToBallDistance < acceptablePToBDistance) {
				KickBall();
				print("kicked ball");
			}
			kickPower= 0;
		}

		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "PlayerJump" ) );
		}


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
		if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

}