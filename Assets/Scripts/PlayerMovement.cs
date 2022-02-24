using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private BoxCollider2D playerCollider;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

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
