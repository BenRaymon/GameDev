using UnityEngine;

/**

TODO

1. Add damage animations

*/

public class PlayerMovementController : MonoBehaviour
{
    // GetComponent Setup
    private Rigidbody2D rb2d;
    private BoxCollider2D playerCollider;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask targetLayer;

    // Used for isGround() to extend raycast further from the player
    private float extraHeightTest = 0.02f;

    // Setup a timer before entering fall state
    private float timeBeforeFall = 0.08f;
    private float timeBeforeFallDelta;

    // General player stats
    private float moveSpeed = 2f;
    private float jumpForce = 40f;

    // Setup for charged jumping
    private float chargeCounter = 0f;
    private bool forcedJump = false;
    private bool chargedJump = false;
    private bool isChargedAttack = false;

    // Setup for registering movement
    private float horizontalMovement;

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
            regularAttack();
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
                chargeCounter += Time.deltaTime;
            }
            
            // forces a jump if held too long
            if(chargeCounter > 2f)
            {
                forcedJump = true;
                chargeCounter = 0f;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space) && isGrounded())
        {
            chargedJump = true;
        }
    }

    private void movePlayer()
    {
        // checks to see if the player is pressing any of the movement keys
        if((horizontalMovement > .1f || horizontalMovement < -.1f) && chargeCounter < .5f)
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
            rb2d.AddForce(new Vector2(0f, jumpForce * 1.8f), ForceMode2D.Impulse);
            forcedJump = false;
            isChargedAttack = true;
        }

        if(chargedJump)
        {
            // did not charge long enough, performs regular jump
            if(chargeCounter < .5f)
            {
                rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                rb2d.AddForce(new Vector2(0f, jumpForce + 10f * chargeCounter), ForceMode2D.Impulse);
            }
            chargedJump = false;
            chargeCounter = 0f;
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
    private void regularAttack()
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

    private void chargedAttack()
    {
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, 5f, targetLayer);

        foreach(Collider2D obj in objectsHit)
        {
            if(obj.tag == "Enemy")
            {
                obj.GetComponent<EnemyController>().takeDamage(100);
            } 
        }
        CinemachineCameraShake.Instance.shakeCamera(1f, 1f);
        isChargedAttack = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
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

        if(chargeCounter > .5f)
        {
            playerState = characterState.chargingJump;
            playerStateText.text = "charging";
            CinemachineCameraShake.Instance.shakeCamera(chargeCounter, .1f);
        }

        if(playerState != characterState.falling && playerState != characterState.jumping)
            timeBeforeFallDelta = timeBeforeFall;

        playerAnimator.SetInteger("state", (int)playerState);
    }

    void OnCollisionEnter2D()
    {
        if(isChargedAttack)
            chargedAttack();
    }
}
