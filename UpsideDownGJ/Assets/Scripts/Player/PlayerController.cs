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
    public Transform itemHolder;

    private GameController gameController;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private bool jumped;

    private GrabbableItem closest;
    private GrabbableItem equipped;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumped = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameController.SwitchGravity();

            spriteRenderer.flipY = gameController.switched;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EquipItem(closest);
        } 
    }

    void FixedUpdate()
    {
        config.isGrounded = IsGrounded(transform.position, groundBottomCheck.position) || IsGrounded(transform.position, groundTopCheck.position);

        var horz = horizontalInput * config.moveSpeed * Time.fixedDeltaTime;
        rb.velocity = new Vector2(horz, rb.velocity.y);

        if (horz != 0)
        {
            spriteRenderer.flipX = horz < 0;
        }

        if (jumped && config.isGrounded)
        {
            var dir = 1;
            if (gameController.switched)
            {
                dir = -1;
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

    private void EquipItem(GrabbableItem item)
    {
        if (equipped != null)
        {
            equipped.ResetItem();
            equipped.Enable();
        }

        closest = null;

        equipped = item;
        if (item == null)
        {
            return;
        }

        item.ResetItem();
        item.Disable();
        item.Equip(itemHolder);
    }

    public void SetClosestGrabbableItem(GrabbableItem item)
    {
        if (closest == item)
        {
            return;
        }

        if (closest != null)
        {
            closest.ResetItem();
        }
        
        closest = item;
    }

    public GrabbableItem GetClosestGrabbableItem()
    {
        return closest;
    }

    public bool FlippedX()
    {
        return spriteRenderer.flipX;
    }

    public bool FlippedY()
    {
        return spriteRenderer.flipY;
    }
}
