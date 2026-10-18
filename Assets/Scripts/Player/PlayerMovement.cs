using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public int facing = 1;   // 1 = right, -1 = left

    Rigidbody2D rb;
    BoxCollider2D col;
    float moveInput;
    bool jumpPressed;
    float speedMultiplier = 1f;
    float slowEndTime;
    SpriteRenderer sr;
    Color originalColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    void Update()
    {
        if (speedMultiplier != 1f && Time.time >= slowEndTime)
        {
            speedMultiplier = 1f;
            if (sr != null) sr.color = originalColor;   // slow wore off, back to normal color
        }
        moveInput = 0f;
        if (Input.GetKey(leftKey)) moveInput -= 1f;
        if (Input.GetKey(rightKey)) moveInput += 1f;

        if (Input.GetKeyDown(jumpKey))
            jumpPressed = true;

        if (moveInput != 0f)
            facing = moveInput > 0f ? 1 : -1;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed * speedMultiplier, rb.linearVelocity.y);

        if (jumpPressed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        jumpPressed = false;
    }

    bool IsGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.05f, groundLayer);
    }

    public void ApplySlow(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        slowEndTime = Time.time + duration;
        if (sr != null) sr.color = Color.Lerp(originalColor, new Color(0.5f, 0.8f, 1f), 0.4f);  // icy blue tint
    }
}