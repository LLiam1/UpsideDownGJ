using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
   
    public PlayerConfiguration config;
    public Transform itemHolder;
    public GameObject characterHolder;

    private GameController gameController;
    private SpriteRenderer spriteRenderer;

    private bool jumped;
    private float jumpTimer;
    private bool falling;

    private Vector3 originalScale;

    public GrabbableItem closest;
    public GrabbableItem equipped;

    public GameObject pauseMenu;
    public bool isPaused;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = characterHolder.GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
        falling = false;

        originalScale = new Vector3(characterHolder.transform.localScale.x, characterHolder.transform.localScale.y, characterHolder.transform.localScale.z);
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isPaused == false)
        {
            //Pause Game!
            pauseMenu.SetActive(true);
            isPaused = true;
            config.isGamePaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            ResumeGame();
        }

        if (!config.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SoundManager.i.PlaySound("gravity_switch");
                falling = true;
                rb.gravityScale = gameController.GetGravityDirection();
                gameController.SwitchGravity();
                spriteRenderer.flipY = gameController.switched;
            }

            var raycastDir = Vector2.down;

            if (gameController.switched)
            {
                raycastDir = Vector2.up;
            }

            var wasOnGround = config.isGrounded;
            config.isGrounded = Physics2D.Raycast(transform.position + config.colliderOffset, raycastDir, config.groundLength, config.groundLayer)
                || Physics2D.Raycast(transform.position - config.colliderOffset, raycastDir, config.groundLength, config.groundLayer)
                || Physics2D.Raycast(transform.position + config.colliderOffset, Util.GetDirectionVector2D(config.coyoteTimeAngleLeft) * raycastDir, config.groundAngleLength, config.groundLayer)
                || Physics2D.Raycast(transform.position - config.colliderOffset, Util.GetDirectionVector2D(config.coyoteTimeAngleRight) * raycastDir, config.groundAngleLength, config.groundLayer);

            if (!wasOnGround && config.isGrounded)
            {
                SoundManager.i.PlaySound("land");
                StartCoroutine(JumpSqueeze(config.landSqueeze.xSqueeze, config.landSqueeze.ySqueeze, config.landSqueeze.seconds));
            }

            jumped = Input.GetKeyDown(KeyCode.Space);
            if (jumped && config.isJumpEnabled)
            {
                jumpTimer = Time.time + config.jumpDelay;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SoundManager.i.PlaySound("equip");
                EquipItem(closest);
            }

            config.direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    void FixedUpdate()
    {
        if (!config.isGamePaused)
        {
            var horz = config.direction.x * config.moveSpeed * Time.fixedDeltaTime;
            rb.velocity = new Vector2(horz, rb.velocity.y);

            if (horz != 0)
            {
                spriteRenderer.flipX = horz < 0;
            }

            if (jumped && config.isGrounded && jumpTimer > Time.time)
            {
                SoundManager.i.PlaySound("jump");
                rb.velocity = new Vector2(rb.velocity.x, config.jumpForce * gameController.GetGravityDirection());
                jumpTimer = 0;
                StartCoroutine(JumpSqueeze(config.jumpSqueeze.xSqueeze, config.jumpSqueeze.ySqueeze, config.jumpSqueeze.seconds));
            }

            ModifyPhysics();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            SoundManager.i.PlaySound("death");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ModifyPhysics()
    {
        bool changingDirections = (config.direction.x > 0 && rb.velocity.x < 0) || (config.direction.x < 0 && rb.velocity.x > 0);

        if (config.isGrounded)
        {
            falling = false;
            if (Mathf.Abs(config.direction.x) < 0.4f || changingDirections)
            {
                rb.drag = config.linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            var gravityDirection = gameController.GetGravityDirection();
            rb.gravityScale = config.gravity * gravityDirection;
            rb.drag = config.linearDrag * 0.15f;

            var yVelocity = gravityDirection < 0 ? rb.velocity.y * -1 : rb.velocity.y;
            if (falling && !config.isGrounded)
            {
                rb.gravityScale = config.gravity * gravityDirection;
            }
            else if (yVelocity < 0)
            {
                rb.gravityScale = config.gravity * gravityDirection * config.fallMultiplier;
            }
            else if (yVelocity > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = config.gravity * gravityDirection * (config.fallMultiplier / 2);
            }
        }
    }

    private void EquipItem(GrabbableItem item)
    {
        if (equipped != null)
        {
            equipped.ResetItem();
            equipped.Enable();
            equipped.SetGravityScale(gameController.GetGravityDirection());
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

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalScale.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalScale, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalScale, t);
            yield return null;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var raycastDir = Vector3.down;
        if (gameController != null && gameController.switched)
        {
            raycastDir = Vector3.up;
        }

        var angleLeft = Util.GetDirectionVector2D(config.coyoteTimeAngleLeft);
        var angleRight = Util.GetDirectionVector2D(config.coyoteTimeAngleRight);

        Gizmos.DrawLine(transform.position + config.colliderOffset, transform.position + config.colliderOffset + raycastDir * config.groundLength);
        Gizmos.DrawLine(transform.position - config.colliderOffset, transform.position - config.colliderOffset + raycastDir * config.groundLength);
        Gizmos.DrawLine(transform.position - config.colliderOffset, transform.position - config.colliderOffset + new Vector3(angleLeft.x, angleLeft.y * raycastDir.y) * config.groundAngleLength);
        Gizmos.DrawLine(transform.position + config.colliderOffset, transform.position + config.colliderOffset + new Vector3(angleRight.x, angleRight.y * raycastDir.y) * config.groundAngleLength);

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        config.isGamePaused = false;
    }
}
