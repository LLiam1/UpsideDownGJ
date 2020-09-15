using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
   
    public PlayerConfiguration config;
    public Transform groundBottomCheck;
    public Transform groundTopCheck;

    private GameController gameController;

    private float horizontalInput;
    private bool jumped;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumped = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameController.SwitchGravity();
        }
    }

    void FixedUpdate()
    {
        config.isGrounded = IsGrounded(transform.position, groundBottomCheck.position) || IsGrounded(transform.position, groundTopCheck.position);

        var horz = horizontalInput * config.moveSpeed * Time.fixedDeltaTime;
        rb.velocity = new Vector2(horz, rb.velocity.y);

        if (jumped && config.isGrounded)
        {
            var dir = -1;
            if (!gameController.switched)
            {
                dir = 1;
            }

            rb.velocity = new Vector2(rb.velocity.x, config.jumpForce * dir);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private bool IsGrounded(Vector3 player, Vector3 groundCheck)
    {
        return Physics2D.Linecast(player, groundCheck, 1 << LayerMask.NameToLayer("Ground"));
    }
}
