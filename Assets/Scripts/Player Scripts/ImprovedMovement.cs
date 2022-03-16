using UnityEngine;

public class ImprovedMovement : MonoBehaviour
{

	/*

	NOTE: Player's state management text flips with the player.
		FIX: Reorganize player container so that the player sprite and text are both children of the container. That way, the sprite can independently flip.

	*/

	// COMPONENTS
	private Rigidbody2D playerBody;
	private CapsuleCollider2D playerCollider;
	private Animator playerAnimator;

	// JUMP SETUP
	private bool isChargedAttack = false;
	private float chargeTimer = 0f;
	private float coyoteTimer; // used for jump forgiveness after leaving the ground
	private float jumpBufferTimer; // used for queueing a jump

	// INPUT PARAMS (A/D and >/<)
	private float horizontalMovement;

	// STATE MANAGEMENT
	private enum characterState {idle, running, jumping, falling, chargingJump}
	private characterState playerState;
	private TextMesh playerStateText;
	private bool isFacingRight = true; // used to detect facing direction
	private bool isJumping = false; // used to move jumping to FixedUpdate()
	private bool queuedJump = false; // used for queueing a jump

    void Awake()
	{		
		playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerStateText = GetComponentInChildren<TextMesh>();
        playerAnimator = GetComponent<Animator>();
	}

	void Start()
	{
		setGravityScale(PlayerData.GRAVITY); // sets default gravity
	}

	void Update()
	{
		horizontalMovement = Input.GetAxisRaw("Horizontal");

		checkFacingDirection(horizontalMovement > 0f); // checks facing direction based on input

		checkForJump(); // checks keys for jump while also handling coyote time and jump queueing

		updatePlayerState(); // state machine update

		if (playerBody.velocity.y >= 0)
            setGravityScale(PlayerData.GRAVITY); // sets default gravity when jumping
		else
			setGravityScale(PlayerData.GRAVITY * PlayerData.FALL_GRAVITY_MULT); // sets higher gravity when falling
	}

	void FixedUpdate()
	{
		// checks for jump
		if(isJumping)
		{
			jumpPlayer(chargeTimer);
		}

		// Continuously update player's movement
		if(chargeTimer < .5f)
		{
			movePlayer();
		}

		// Applies different drag depending on the situation
		if(playerState == characterState.jumping || playerState == characterState.falling)
		{
			Drag(PlayerData.AIR_DRAG);
		}
		else
		{
			Drag(PlayerData.GROUND_DRAG);
		}
	}

	private void movePlayer()
	{
		float targetSpeed = horizontalMovement * PlayerData.MOVE_SPEED; // can be 0, -MOVE_SPEED, or +MOVE_SPEED

		// takes the difference to find out how much force should be applied. Larger differences mean larger forces (such as when the player wants to turn)
		float speedDiff = targetSpeed - playerBody.velocity.x;

		// How fast the player should accelerate. Equivalent to how we accelerate when we start to run.
		float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PlayerData.RUN_ACCEL : PlayerData.RUN_DECELL;

		// determines the curve at which the player speeds up. Used to make movements feel more natural.
		float velocityPow;
		if (Mathf.Abs(targetSpeed) < 0.01f)
		{
			velocityPow = PlayerData.STOP_POWER;
		}
		else if (Mathf.Abs(playerBody.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(playerBody.velocity.x)))
		{
			velocityPow = PlayerData.TURN_POWER;
		}
		else
		{
			velocityPow = PlayerData.ACCEL_POWER;
		}

		// force to be applied
		float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPow) * Mathf.Sign(speedDiff);

		playerBody.AddForce(movement * Vector2.right);
	}

	private void checkForJump()
	{
		bool grounded = isGrounded(); // ground check
		jumpBufferTimer -= Time.deltaTime;

		// Coyote time controller
		if(grounded)
		{
			coyoteTimer = PlayerData.COYOTE_TIME;
		}
		else if(coyoteTimer > -1f)
		{
			coyoteTimer -= Time.deltaTime;
		}

		#region QUEUED JUMPING
		if(Input.GetKeyDown(KeyCode.Space) && !queuedJump && playerState == characterState.falling)
		{
			queuedJump = true;
			jumpBufferTimer = PlayerData.JUMP_QUEUE;
		} 
		else if(jumpBufferTimer > -1f)
		{
			jumpBufferTimer -= Time.deltaTime;
		}

		// used to separate queued jumping from regular jump. If a jump is queued and conditions are met,
		// a jump should automatically be executed the next time the player is grounded.
		if(queuedJump)
		{
			if(jumpBufferTimer > 0f && grounded)
			{
				isJumping = true;
			}
		}
		#endregion

		if(Input.GetKey(KeyCode.Space) && grounded)
		{
			chargeJump();
		}

		if(Input.GetKeyUp(KeyCode.Space) && (grounded || coyoteTimer > 0f))
		{
			isJumping = true;
		}
	}

	private void chargeJump()
	{
		if (chargeTimer < 2f)
		{
			chargeTimer += Time.deltaTime;
		}
			
		// forces a jump if held too long (2 seconds)
		if(chargeTimer > 2f)
		{
			isJumping = true;
			isChargedAttack = true;
		}
	}

	private void jumpPlayer(float buffer)
	{
		if(buffer < 0.5f)
		{
            playerBody.AddForce(new Vector2(0f, PlayerData.JUMP_FORCE), ForceMode2D.Impulse);
        } 
		else 
		{
            playerBody.AddForce(new Vector2(0f, PlayerData.JUMP_FORCE * buffer), ForceMode2D.Impulse);
        }

		// reset values
		isJumping = false;
		queuedJump = false;
		chargeTimer = 0f;
		coyoteTimer = 0f;
	}

	private void checkFacingDirection(bool isMovingRight)
	{
		if(isMovingRight != isFacingRight)
		{
			Turn();
		}
	}

	private void Turn()
	{
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

		isFacingRight = !isFacingRight;
	}
	
	private void setGravityScale(float scale)
	{
		playerBody.gravityScale = scale;
	}

	// determines direction to apply drag in and applies it.
	private void Drag(float amount)
	{
		Vector2 force = amount * playerBody.velocity.normalized;
		force.x = Mathf.Min(Mathf.Abs(playerBody.velocity.x), Mathf.Abs(force.x)); 
		force.y = Mathf.Min(Mathf.Abs(playerBody.velocity.y), Mathf.Abs(force.y));
		force.x *= Mathf.Sign(playerBody.velocity.x);
		force.y *= Mathf.Sign(playerBody.velocity.y);

		playerBody.AddForce(-force, ForceMode2D.Impulse);
	}

	// Raycast grounding test for both of the player's "feet"
	private bool isGrounded()
	{
		GameObject isLeftGrounded = collisionDetector(playerCollider.bounds.min, Vector2.down, PlayerData.GROUND_BUFFER);
        GameObject isRightGrounded = collisionDetector(new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y), Vector2.down, PlayerData.GROUND_BUFFER);

		return(isLeftGrounded || isRightGrounded);
	}

	// Raycast function, returns the gameObject belonging to the collider the raycast hits.
	private GameObject collisionDetector(Vector2 origin, Vector2 direction, float distance){
        RaycastHit2D rayCast = Physics2D.Raycast(origin, direction, distance);
        GameObject objectHit = rayCast.collider?.gameObject;

        return objectHit;
    }

	private void updatePlayerState()
    {
        if((playerBody.velocity.x > .1f || playerBody.velocity.x < -.1f) && Mathf.Approximately(playerBody.velocity.y, 0f))
        {        
            playerState = characterState.running;
            playerStateText.text = "running";
        }
        else if(Mathf.Approximately(playerBody.velocity.x, 0f) && Mathf.Approximately(playerBody.velocity.y, 0f))
        {
            playerState = characterState.idle;
            playerStateText.text = "idle";
        }
        
        if(playerBody.velocity.y > .1f)
        {
            playerState = characterState.jumping;
            playerStateText.text = "jumping";
        }
        
        if(playerBody.velocity.y < -.1f)
        {
			playerState = characterState.falling;
			playerStateText.text = "falling";
        }

        if(chargeTimer > .5f)
        {
            playerState = characterState.chargingJump;
            playerStateText.text = "charging";
            //CinemachineCameraShake.Instance.shakeCamera(chargeTimer/10, .1f);
		}

        playerAnimator.SetInteger("state", (int)playerState);
    }

}