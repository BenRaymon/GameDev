using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D playerCollider;
    private float moveSpeed = 2f;
    private float jumpForce = 30f;
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
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        updatePlayerState();
    }

    void FixedUpdate()
    {
        if(horizontalMovement > .1f || horizontalMovement < -.1f)
        {
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

        float extraHeightTest = 0.015f;

        // Casts a ray from the center-bottom of the player's box collider
        RaycastHit2D rayCastTest = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightTest);

        return rayCastTest;
    }

    private void updatePlayerState()
    {
        if(horizontalMovement > .1f || horizontalMovement < -.1f)
        {        
            playerState = characterState.running;
            playerStateText.text = "running";
        }
        else
        {
            playerState = characterState.idle;
            playerStateText.text = "idle";
        }
        
        if(verticalMovement > .1f)
        {
            playerState = characterState.jumping;
            playerStateText.text = "jumping";
        }
        
        if(rb2d.velocity.y < -.1f)
        {
            playerState = characterState.falling;
            playerStateText.text = "falling";
        }
    }
}
