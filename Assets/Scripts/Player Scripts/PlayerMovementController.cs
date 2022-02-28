using UnityEngine;

/**

TODO

1. Add a delay to jumping
2. Change attackEnemy() to use Physics2D.OverlapCircle to simulate AOE damage from landing
    2a. Possibly change regular jumping to quick jump and reduce damage done. Increase damage for charged jumps.
3. Add damage animations

*/

// NOTE: Player jumping feels sticky and can cause player death if the jump key is not pressed and released instantly.

public class PlayerMovementController : MonoBehaviour
{
    // GetComponent Setup
    private Rigidbody2D rb2d;
    private BoxCollider2D playerCollider;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;

    // Used for isGround() to extend raycast further from the player
    private float extraHeightTest = 0.02f;

    // Setup a timer before entering fall state
    private float timeBeforeFall = 0.08f;
    private float timeBeforeFallDelta;

    // General player stats
    private float moveSpeed = 2f;
    private float jumpForce = 40f;

    // Setup for charged jumping
    private int chargeCounter = 0;
    private bool forcedJump = false;
    private bool chargedJump = false;

    // Setup for registering movement
    private float horizontalMovement;
    private float verticalMovement;

    // Setup for player state management
    private enum characterState {idle, running, jumping, falling, chargingJump}
    // Can be repurposed to display player health later
    private TextMesh playerStateText;
    private characterState playerState;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerStateText = GetComponentInChildren<TextMesh>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

        timeBeforeFallDelta = timeBeforeFall;
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        checkForJump();

        updatePlayerState();

        if(playerState == characterState.falling)
            attackEnemy();
    }

    void FixedUpdate()
    {
        movePlayer();
        jumpPlayer();
    }

    private void checkForJump()
    {
        if(Input.GetKey(KeyCode.Space) && isGrounded())
        {
            if(!forcedJump)
            {
                chargeCounter += 1;
                CinemachineCameraShake.Instance.shakeCamera(chargeCounter/100, .1f);
            }
            
            // forces a jump if held too long
            if(chargeCounter > 500)
            {
                chargeCounter = 0;
                forcedJump = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space) && isGrounded())
        {
            chargedJump = true;
            chargeCounter = 0;
        }
    }

    private void movePlayer()
    {
        // checks to see if the player is pressing any of the movement keys
        if((horizontalMovement > .1f || horizontalMovement < -.1f) && chargeCounter < 120)
        {
            // flips sprite so it is facing the direction of movement
            if(horizontalMovement > .1f)
                playerSprite.flipX = false;
            else if(horizontalMovement < -.1f)
                playerSprite.flipX = true;

            rb2d.AddForce(new Vector2(horizontalMovement * moveSpeed, 0f), ForceMode2D.Impulse);
        }
    }

    private void jumpPlayer()
    {
        if(forcedJump)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce + 500/20), ForceMode2D.Impulse);
            forcedJump = false;
        }

        if(chargedJump)
        {
            if(chargeCounter < 100)
                rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            else
                rb2d.AddForce(new Vector2(0f, jumpForce + chargeCounter/20), ForceMode2D.Impulse);
            chargedJump = false;
        }
    }

    private bool isGrounded()
    {
        // Tests if a player is grounded by casting a ray and checking if the ray is colliding with any existing colliders.

        // Casts a ray from the center-bottom of the player's box collider
        RaycastHit2D rayCastGroundTest = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightTest);

        return rayCastGroundTest;
    }

    // 
    // Uses the same raycast as isGrounded. Retrieves whatever gameobject collides with the raycast and checks if it is an enemy.
    // If it is an enemy, get the enemycontroller script component of that gameobject and calls the function takeDamage.
    //
    private void attackEnemy()
    {
        //Check for collision on the left side of the player
        GameObject collisionLeft = collisionDetector(playerCollider.bounds.min, Vector2.down, extraHeightTest);
        //Check for collision on the right side of the player
        GameObject collisionRight = collisionDetector(new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y), Vector2.down, extraHeightTest);
        
        if(collisionLeft && collisionLeft.tag == "Enemy"){
            collisionLeft.GetComponent<EnemyController>().takeDamage(100);
        }
        else if(collisionRight && collisionRight.tag == "Enemy"){
            collisionRight.GetComponent<EnemyController>().takeDamage(100);
        }

    }

    //This function takes in 
    //  an origin vector (the starting position of the ray)
    //  a direction vector (the direction to cast the ray)
    //  a distance float (the distance to cast the ray)
    //The function returns the object hit by the ray (or null if no object is hit)  
    private GameObject collisionDetector(Vector2 origin, Vector2 direction, float distance){
        RaycastHit2D rayCast = Physics2D.Raycast(origin, direction, distance);
        GameObject objectHit = rayCast.collider?.gameObject;

        return objectHit;
    }

    private void updatePlayerState()
    {
        if((rb2d.velocity.x > .1f || rb2d.velocity.x < -.1f) && Mathf.Approximately(rb2d.velocity.y, 0f))
        {        
            playerState = characterState.running;
            playerStateText.text = "running";
        }
        else if(Mathf.Approximately(rb2d.velocity.x, 0f) && Mathf.Approximately(rb2d.velocity.y, 0f))
        {
            playerState = characterState.idle;
            playerStateText.text = "idle";
        }
        
        if(rb2d.velocity.y > .1f)
        {
            playerState = characterState.jumping;
            playerStateText.text = "jumping";
        }
        
        if(rb2d.velocity.y < -.1f)
        {
            if(timeBeforeFallDelta >= 0f)
                timeBeforeFallDelta -= Time.deltaTime;
            else
            {
                playerState = characterState.falling;
                playerStateText.text = "falling";
            }
        }

        if(chargeCounter > 120)
        {
            playerState = characterState.chargingJump;
            playerStateText.text = "charging";
        }

        if(playerState != characterState.falling && playerState != characterState.jumping)
            timeBeforeFallDelta = timeBeforeFall;

        playerAnimator.SetInteger("state", (int)playerState);
    }
}
