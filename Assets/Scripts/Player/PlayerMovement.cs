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
    public enum InputMode { Keyboard, Arduino }

    [Header("Who is this")]
    public int playerNumber = 1;                    // 1 or 2
    public InputMode inputMode = InputMode.Keyboard;

    Rigidbody2D rb;
    CapsuleCollider2D col;
    float moveInput;
    bool jumpPressed;
    float speedMultiplier = 1f;
    float slowEndTime;
    SpriteRenderer sr;
    Color originalColor;

    public int objectiveDirection = 1;
    public float carrySpeedMultiplier = 0.75f;   // 0.75 = 25% slower while holding the package

    float boostMultiplier = 1f;
    float boostEndTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        PhysicsMaterial2D slick = new PhysicsMaterial2D("PlayerSlick");
        slick.friction = 0f;
        slick.bounciness = 0f;
        col.sharedMaterial = slick;
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    void ReadInput()
    {
        if (inputMode == InputMode.Keyboard)
        {
            moveInput = 0f;
            if (Input.GetKey(leftKey)) moveInput -= 1f;
            if (Input.GetKey(rightKey)) moveInput += 1f;
            if (Input.GetKeyDown(jumpKey)) jumpPressed = true;
        }
        else if (inputMode == InputMode.Arduino)
        {
            // Arduino
            if (Aurdino.newYaw < -4) moveInput -= 1f;
            if (Aurdino.newYaw > 4) moveInput += 1f;
        }
    }

    void Update()
    {
        if (boostMultiplier != 1f && Time.time >= boostEndTime)
            boostMultiplier = 1f;

        if (speedMultiplier != 1f && Time.time >= slowEndTime)
        {
            speedMultiplier = 1f;
            if (sr != null) sr.color = originalColor;   // slow wore off, back to normal color
        }
        moveInput = 0f;

        if (inputMode == InputMode.Keyboard)
        {
            moveInput = 0f;
            if (Input.GetKey(leftKey)) moveInput -= 1f;
            if (Input.GetKey(rightKey)) moveInput += 1f;
            if (Input.GetKeyDown(jumpKey)) jumpPressed = true;
        }
        else if (inputMode == InputMode.Arduino)
        {
            // Arduino
            if (Aurdino.newYaw < -4) moveInput -= 1f;
            if (Aurdino.newYaw > 4) moveInput += 1f;
        }

        if (moveInput != 0f)
            facing = moveInput > 0f ? 1 : -1;
    }

    void FixedUpdate()
    {
        float carry = (Package.Instance != null && Package.Instance.holder == this) ? carrySpeedMultiplier : 1f;
        rb.linearVelocity = new Vector2(moveInput * moveSpeed * speedMultiplier * carry * boostMultiplier, rb.linearVelocity.y);

        if (jumpPressed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        jumpPressed = false;
    }
    public void ApplyBoost(float multiplier, float duration)
    {
        boostMultiplier = multiplier;
        boostEndTime = Time.time + duration;
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
    public void Teleport(Vector2 pos)
    {
        transform.position = pos;
        rb.position = pos;
    }

    public void ResetState()
    {
        boostMultiplier = 1f;
        rb.linearVelocity = Vector2.zero;
        speedMultiplier = 1f;                       // clears any freeze slow
        if (sr != null) sr.color = originalColor;   // clears the tint
        facing = objectiveDirection;
    }
}