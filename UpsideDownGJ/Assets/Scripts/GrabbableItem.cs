using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class GrabbableItem : MonoBehaviour, IGravityObject
{
    public float highlightRange;
    public float distanceToPlayer;
    public bool isClosest;

    private PlayerController player;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Rigidbody2D rb;
    private Collider2D collider2D;

    private bool useGravity = true;
    private float lastGravityScale;

    private Transform holder;
    private bool isEquipped;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        isClosest = false;
        isEquipped = false;
        useGravity = true;

        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEquipped)
        {
            transform.position = holder.position;
            return;
        }

        // for some reason isEquipped is getting set to false somewhere and is throwing off these calculations so need to
        // return here if the rigidbody isn't being simulated.
        if (!rb.simulated)
        {
            return;
        }

        distanceToPlayer = Util.CalculateDistance(player.transform.position, transform.position);

        var color = originalColor;

        var currClosest = player.GetClosestGrabbableItem();
        var currClosestDistance = Mathf.Infinity;
        if (currClosest != null)
        {
            currClosestDistance = currClosest.distanceToPlayer;
        }

        if (distanceToPlayer <= highlightRange && currClosestDistance >= distanceToPlayer)
        {
            color = Color.green;
            isClosest = true;
            player.SetClosestGrabbableItem(this);
        } else if (isClosest)
        {
            player.SetClosestGrabbableItem(null);
        }

        spriteRenderer.color = color;
    }

    public void ResetItem()
    {
        holder = null;
        isEquipped = false;
        spriteRenderer.color = originalColor;
        isClosest = false;
    }

    public void Disable()
    {
        rb.simulated = false;
        collider2D.enabled = false;
    }

    public void Enable()
    {
        rb.simulated = true;
        collider2D.enabled = true;
    }

    public void Equip(Transform holder)
    {
        isEquipped = true;
        this.holder = holder;
    }

    public void SetGravityScale(float gravityScale)
    {
        Debug.Log($"Setting gravity scale to {gravityScale} for {gameObject.name}");
        rb.gravityScale = gravityScale;
    }

    public bool UsingGravity()
    {
        return useGravity;
    }

    public void DisableGravity()
    {
        useGravity = false;
        lastGravityScale = rb.gravityScale;
        SetGravityScale(0);
    }

    public void EnableGravity()
    {
        useGravity = true;
        SetGravityScale(lastGravityScale);
    }

    public float GetGravityScale()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        return rb.gravityScale;
    }
}
