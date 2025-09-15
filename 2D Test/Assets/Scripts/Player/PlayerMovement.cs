using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;
    public float jumpGraceTime = 0.15f; // coyote time

    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastGroundedTime;
    private float jumpBufferTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        jumpBufferTime = -999f; // Should prevent jumping right when it starts
    }

    void Update()
    {
        if (!GlobalVariables.focusLocked)
        {
            // check to make suer the player is on the ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
            if (isGrounded) { lastGroundedTime = Time.time; }

            // this should allow jump if a player presses it slightly before landing
            if (Input.GetKeyDown(KeyCode.Space)) { jumpBufferTime = Time.time; }
            

            // Horizontal movement
            float moveInput = 0f;
            if (Input.GetKey(KeyCode.A)) { moveInput -= 1f; }
            if (Input.GetKey(KeyCode.D)) { moveInput += 1f; }
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            // Jump
            if (Time.time - jumpBufferTime < jumpGraceTime && Time.time - lastGroundedTime < jumpGraceTime)
            {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferTime = -999f; // Prevent double jump
            }
        }
        
    }
}