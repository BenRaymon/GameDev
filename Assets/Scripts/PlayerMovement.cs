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

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if(horizontalMovement > .1f || horizontalMovement < -.1f)
            rb2d.AddForce(new Vector2(horizontalMovement * moveSpeed, 0f), ForceMode2D.Impulse);
        if(verticalMovement > .1f && isGrounded())
            rb2d.AddForce(new Vector2(0f, verticalMovement * jumpForce), ForceMode2D.Impulse);
    }

    private bool isGrounded()
    {
        // Tests if a player is grounded by casting a ray and checking if the ray is colliding with any existing colliders.
        // 'Queries start in colliders' is disabled in Project Settings -> Physics2D in order for raycast to ignore its own collider.

        float extraHeightTest = 0.02f;

        // Casts a ray from the center-bottom of the player's box collider
        RaycastHit2D rayCastTest = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeightTest);

        // Will return null if there is no collision between the ray and a collider.
        return rayCastTest.collider != null;
    }
}
