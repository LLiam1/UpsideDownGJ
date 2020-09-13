using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
   
    public PlayerConfiguration config;
    public Transform groundBottomCheck;
    public Transform groundTopCheck;
    public bool switched;

    private float horizontalInput;
    private bool jumped;
    private bool switchedGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        switched = false;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumped = Input.GetKeyDown(KeyCode.Space);
        switchedGravity = Input.GetKeyDown(KeyCode.Q);
    }

    void FixedUpdate()
    {
        config.isGrounded = IsGrounded(transform.position, groundBottomCheck.position) || IsGrounded(transform.position, groundTopCheck.position);

        var horz = horizontalInput * config.moveSpeed * Time.fixedDeltaTime;
        rb.velocity = new Vector2(horz, rb.velocity.y);

        if (jumped && config.isGrounded)
        {
            var dir = -1;
            if (!switched)
            {
                dir = 1;
            }

            rb.velocity = new Vector2(rb.velocity.x, config.jumpForce * dir);
        }

        if (switchedGravity && config.isGrounded)
        {
            switched = !switched;
            rb.gravityScale = rb.gravityScale * -1;
        }
    }

    private bool IsGrounded(Vector3 player, Vector3 groundCheck)
    {
        return Physics2D.Linecast(player, groundCheck, 1 << LayerMask.NameToLayer("Ground"));
    }
}
