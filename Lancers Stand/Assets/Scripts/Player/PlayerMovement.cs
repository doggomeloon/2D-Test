using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;

    [Header("Jump Settings")]
    
    public float minJumpForce = 10f; // 10 unity units i think, minimum jump capability
    public float maxJumpForce = 16f; // max jump
    public float chargeTime = 2f; //amnt of time it takes to get to max jump from min jump
    
    // this if it works allows players to jump before they actually hit the ground
    // almost like they are "pre-loading" their next jump
    public float jumpGraceTime = 0.15f;
    
    private float jumpBufferTime;

    [Header("Ground Check Stuff")] 
    // All this stuff is to just make sure that they player is actually touching the ground before jumping
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastGroundedTime;

    // Charging stuff
    private bool isChargingJump; // Is holding spacebar or other jump key ig but who doesnt use spacebar for jump
    private float jumpCharge; // Current stop inbetween min and max


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpBufferTime = -999f; // Should prevent jumping right when it starts
    }

    void Update()
    {
        if (!GlobalVariables.focusLocked)
        {

            // Horizontal movement
            float moveInput = 0f;
            if (Input.GetKey(GlobalVariables.leftKey)) { moveInput -= 1f; }
            if (Input.GetKey(GlobalVariables.rightKey)) { moveInput += 1f; }
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            // check to make suer the player is on the ground before jumping
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
            if (isGrounded) { lastGroundedTime = Time.time; }

            // this should allow jump if a player presses it slightly before landing
            if (Input.GetKeyDown(GlobalVariables.jumpKey)) { jumpBufferTime = Time.time; }

            /// sllows the player to start charging jump before they actually touch the ground
            if (!isChargingJump && Input.GetKeyDown(GlobalVariables.jumpKey) &&
            Time.time - jumpBufferTime < jumpGraceTime &&
            Time.time - lastGroundedTime < jumpGraceTime)
            {
                isChargingJump = true;
                jumpCharge = 0f;
            }

            // Starts increasing the jump charge
            if (isChargingJump && Input.GetKey(GlobalVariables.jumpKey))
            {
                jumpCharge += Time.deltaTime / chargeTime;
                jumpCharge = Mathf.Clamp01(jumpCharge);
            }

            // Letting go of spacebar after (or not) charging
            if (isChargingJump && Input.GetKeyUp(GlobalVariables.jumpKey))
            {
                float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge); // We love linear interpolation
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Clears vertical velocity
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Jump!

                isChargingJump = false; // Start over
                jumpBufferTime = -999f; // Hoopefully this prevents double jumping
            }
        }
        
    }
}