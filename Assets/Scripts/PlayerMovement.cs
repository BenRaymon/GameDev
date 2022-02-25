using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**

BUGS

The player can sometimes get stuck in the running state.
    Seen when the player jumps ontop of an enemy without triggering the collision check (landing at the top corners of the enemy).
    After landing there, movement in any direction will cause the player to become stuck in the running state while still on the enemy.
    CAUSE: The player has a x-velocity of +- 1.4 with intermitently changing y velocity when moving ontop of the enemy.

    EDIT: there were two main causes for the error described above
      1. The idle state is not updating because of floating point imprecision
      it is generally considered bad practice to compare floats using equlality (x == 0) 
      because a float may print as 0 and look like 0 but may not be exactly zero (0.00000001)
      FIX: The "best practice" would be to use approximation, in C# that would be by using Mathf.Approximately(x, y)

      2. raycasting from the center causing a player to hang on to the edge of an enemy
      when a ray is cast from the center of the player, it doesn't consider the left and right boundaries
      so if a player lands halfway on an enemy (just clipping the side of the enemy)
      it won't register that as a collision.
      FIX: cast a ray from the left and from the right of the player

*/

/**

TODO

1. Add a delay to jumping

*/

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D playerCollider;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;
    private float extraHeightTest = 0.02f;
    private float moveSpeed = 2f;
    private float jumpForce = 50f;
    private float horizontalMovement;
    private float verticalMovement;
    
    // Currently used for debugging purposes
    private enum characterState {idle, running, jumping, falling}
    private TextMesh playerStateText;
    private characterState playerState;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerStateText = GetComponentInChildren<TextMesh>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        updatePlayerState();

        if(playerState == characterState.falling)
            attackEnemy();
    }

    void FixedUpdate()
    {
        if(horizontalMovement > .1f || horizontalMovement < -.1f)
        {
            if(horizontalMovement > .1f)
                playerSprite.flipX = false;
            else if(horizontalMovement < -.1f)
                playerSprite.flipX = true;
            rb2d.AddForce(new Vector2(horizontalMovement * moveSpeed, 0f), ForceMode2D.Impulse);
        }
        if(verticalMovement > .1f && isGrounded())
        {
            rb2d.AddForce(new Vector2(0f, verticalMovement * jumpForce), ForceMode2D.Impulse);
        }
    }

    private bool isGrounded()
    {
        // Tests if a player is grounded by casting a ray and checking if the ray is colliding with any existing colliders.
        // 'Queries start in colliders' is disabled in Project Settings -> Physics2D in order for raycast to ignore its own collider.

        // Casts a ray from the center-bottom of the player's box collider
        RaycastHit2D rayCastGroundTest = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightTest);

        return rayCastGroundTest;
    }

    // 
    // Uses the same raycast as isGrounded. Retrieves whatever gameobject collides with the raycast and checks if it is an enemy.
    // If it is an enemy, get the enemycontroller script component of that gameobject and calls the function takeDamage.
    //
    // <QUESTION> Should this be moved to a separate script attached to the player?
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
        if(horizontalMovement > .1f || horizontalMovement < -.1f)
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
            playerState = characterState.falling;
            playerStateText.text = "falling";
        }

        playerAnimator.SetInteger("state", (int)playerState);
    }
}
