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
    private float GROUND_BUFFER = 0.02f;
    // General player stats
    private float MOVE_SPEED = 2f;
    private float JUMP_FORCE = 40f;

    //Global variable for 
    private bool isChargedAttack = false;
    private float chargeCounter = 0f;

    // Acts as input listeners for A/D and > <
    private float horizontalMovement;

    // Setup for player state management
    private enum characterState {idle, running, jumping, falling, chargingJump}

    // Can be repurposed to display player health later
    private TextMesh playerStateText;
    private characterState playerState;

    // Setup a timer before entering fall state
    //private float timeBeforeFall = 0.08f;
    //private float timeBeforeFallDelta;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerStateText = GetComponentInChildren<TextMesh>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

        //timeBeforeFallDelta = timeBeforeFall;
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
        // checks to see if the player is pressing any of the movement keys
        if((horizontalMovement > .1f || horizontalMovement < -.1f) && chargeCounter < .5f)
            movePlayer();
        
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

    //This function moves the player left or right
    //and flips the sprite if needed
    private void movePlayer()
    {
        // flips sprite so it is facing the direction of movement
        if(horizontalMovement > .1f)
            playerSprite.flipX = false;
        else if(horizontalMovement < -.1f)
            playerSprite.flipX = true;

        rb2d.AddForce(new Vector2(horizontalMovement * MOVE_SPEED, 0f), ForceMode2D.Impulse);
    }

    //Input listeners for Space Bar
    private void checkForJump()
    {
        GameObject isGrounded = collisionDetector(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + GROUND_BUFFER);

        if(Input.GetKey(KeyCode.Space) && isGrounded){
            if (chargeCounter < 1.8f)
                chargeCounter += Time.deltaTime;
            
            // forces a jump if held too long (2 seconds)
            if(chargeCounter > 1.8f){
                jumpPlayer(true, chargeCounter);
                isChargedAttack = true;
                chargeCounter = 0f;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space) && isGrounded){
            jumpPlayer(false, chargeCounter);
            chargeCounter = 0f;
        }
    }

    //This function jumps the player 
    //1. Regular jump, just JUMP_FORCE
    //2. Charged jump, calcualted by JUMP_FORCE*buffer
    private void jumpPlayer(bool isForced, float buffer)
    {
        if(buffer < 0.5f){
            rb2d.AddForce(new Vector2(0f, JUMP_FORCE), ForceMode2D.Impulse);
        } else {
            rb2d.AddForce(new Vector2(0f, JUMP_FORCE * buffer), ForceMode2D.Impulse);
        }
    }

    // This function checks for a collision with an enemy (using Raycast)
    // and attacks the enemy if there is a collision
    private void regularAttack()
    {
        //Check for collision on the left side of the player
        GameObject collisionLeft = collisionDetector(playerCollider.bounds.min, Vector2.down, GROUND_BUFFER);
        //Check for collision on the right side of the player
        GameObject collisionRight = collisionDetector(new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y), Vector2.down, GROUND_BUFFER);
        
        if(collisionLeft && collisionLeft.tag == "Enemy"){
            collisionLeft.GetComponent<EnemyController>().takeDamage(100);
        }
        else if(collisionRight && collisionRight.tag == "Enemy"){
            collisionRight.GetComponent<EnemyController>().takeDamage(100);
        }

    }

    //creates a circle around the player and if any enemies are in the circle they take full damage
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
        CinemachineCameraShake.Instance.shakeCamera(.5f, .2f);
        isChargedAttack = false;
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
            //if(timeBeforeFallDelta >= 0f)
            //    timeBeforeFallDelta -= Time.deltaTime;
            //else
            //{
                playerState = characterState.falling;
                playerStateText.text = "falling";
            //}
        }

        if(chargeCounter > .5f)
        {
            playerState = characterState.chargingJump;
            playerStateText.text = "charging";
            CinemachineCameraShake.Instance.shakeCamera(chargeCounter/10, .1f);
        }

        //if(Mathf.Approximately(rb2d.velocity.y, 0f))
        //    timeBeforeFallDelta = timeBeforeFall;

        playerAnimator.SetInteger("state", (int)playerState);
    }

    void OnCollisionEnter2D()
    {
        if(isChargedAttack)
            chargedAttack();
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

}
