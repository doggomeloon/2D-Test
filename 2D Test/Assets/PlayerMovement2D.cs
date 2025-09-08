using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Move & Jump")]
    public float moveSpeed = 8f;
    public float jumpHeight = 3f;          // in world units

    [Header("Grounding")]
    public Transform groundCheck;          // empty child at feet
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;

    [Header("Feel")]
    public float coyoteTime = 0.1f;        // grace after leaving ground
    public float jumpBuffer = 0.1f;        // grace before landing
    public float accel = 100f;
    public float decel = 100f;
    public float airControl = 0.6f;
    public float maxFallSpeed = -20f;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;

    private float moveInput;               // -1..1
    private float coyoteTimer;
    private float jumpBufferTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;     // smooth render between physics steps
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // New Input System bindings (A/D + arrows)
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Left", "<Keyboard>/a").With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d").With("Right", "<Keyboard>/rightArrow");

        jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");
    }

    void OnEnable(){ moveAction.Enable(); jumpAction.Enable(); }
    void OnDisable(){ moveAction.Disable(); jumpAction.Disable(); }

    void Update()
    {
        // Read input in Update (frame-rate), apply in FixedUpdate (physics)
        moveInput = moveAction.ReadValue<Vector2>().x;

        if (jumpAction.triggered) jumpBufferTimer = jumpBuffer;
        else                      jumpBufferTimer -= Time.unscaledDeltaTime;
    }

    void FixedUpdate()
    {
        // Ground check
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        coyoteTimer = grounded ? coyoteTime : coyoteTimer - Time.fixedDeltaTime;

        // Horizontal movement with accel/decel and reduced air control
        float target = moveInput * moveSpeed;
        float rate = (Mathf.Abs(target) > 0.01f ? accel : decel) * (grounded ? 1f : airControl);
        float vx = Mathf.MoveTowards(rb.linearVelocity.x, target, rate * Time.fixedDeltaTime);

        // Jump if buffered & within coyote window
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float jumpVel = Mathf.Sqrt(2f * g * jumpHeight);
            rb.linearVelocity = new Vector2(vx, jumpVel);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }
        else
        {
            rb.linearVelocity = new Vector2(vx, Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
