using System;
using UnityEngine;

[Serializable]
public class PlayerConfiguration
{
    public float moveSpeed = 1500;
    public float jumpForce = 25;
    public float gravity = 1f;
    public float linearDrag;
    public float fallMultiplier;
    public float jumpDelay = 0.25f;
    public SqueezeConfiguration landSqueeze;
    public SqueezeConfiguration jumpSqueeze;
    public float groundLength = 0.6f;
    public float groundAngleLength = 1.4f;
    public float coyoteTimeAngleLeft = 100f;
    public float coyoteTimeAngleRight = 80f;
    public LayerMask groundLayer;
    public Vector2 direction;
    public Vector3 colliderOffset;
    public bool isGrounded = true;
    public bool isJumpEnabled = true;
    public bool isGamePaused = false;
}
