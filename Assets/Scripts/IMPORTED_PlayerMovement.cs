using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMPORTED_PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private Animator playerAnimation;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D playerCollider;
    private bool jumping = false;
    private bool isMoving = false;
    private float horizontal;
    private enum animationState {idle, jumping, falling};

    public float moveSpeed = 5f;
    public float jumpHeight = 5f;


    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if(horizontal != 0f)
            isMoving = true;

        if(Input.GetButtonDown("Jump") && isGrounded())
            jumping = true;

        updateAnimationState();
        // if(isGrounded() && horizontal == 0f)
        //     playerBody.velocity = Vector2.zero;
    }

    /**
    * B U G S  T O  F I X
    * 
    * 1. Horizontal movement generates y velocity (negative when moving left and positive when moving right)
    *       Unknown cause -- Idle animation could be cause
    *
    * 2. Idle animation generates velocity
    *       Unknown cause -- Could be caused by collider moving?
    *       Unknown effects, though possibly contributes to bug 1
    *
    * 3. Infinite loop where the animation switches between falling and idle.
    *       Unknown cause
    *
    * 4. At times, when the player is idle, jumping does not work. The animation plays the first frame and then immediately
    *    switches back to the idle animation with no significant change in y velocity.
    *       Unknown cause
    *
    */

    void FixedUpdate()
    {

        if(jumping)
        {
            playerBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            // playerBody.velocity = new Vector2(playerBody.velocity.x, jumpHeight);
            jumping = false;
        }

        if(isMoving)
        {
            playerBody.velocity = new Vector2(moveSpeed * horizontal, playerBody.velocity.y);
        }

    }

    private void updateAnimationState()
    {
        animationState state;

        if(horizontal > 0f)
            state = animationState.idle;
        else
            state = animationState.idle;

        if(playerBody.velocity.y > .1f)
            state = animationState.jumping;
        else if(playerBody.velocity.y < -40f)
            state = animationState.falling;

        playerAnimation.SetInteger("state", (int)state);
    }

    private bool isGrounded() 
    {
        float extraHeightTest = .025f;
        RaycastHit2D rayCastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y  + extraHeightTest);

        Color rayColor;
        if(rayCastHit.collider != null) 
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y  + extraHeightTest), rayColor);

        return rayCastHit.collider != null;
    }

}
