using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public int health = 5;

    [Header("Objects")]
    public GameObject player;
    public GameObject enemy;
    public TextMeshProUGUI healthText;
    private Rigidbody2D rb;

    [Header("Knockback (player=player->enemy, enemy = enemy->player)")] // idk why i made this so confusing
    // Player to enemy knockback
    public float knockbackStrength = 5f;
    public float knockbackUpwardForce = 5f; // Add vertical jump to knockback
    // Enemy to player knockback
    public float playerKnockbackStrength = 5f; // Amount that player gets knocked back
    public float playerKnockbackUpwardForce = 5f; // Add vertical jump to knockback
    private Vector2 knockbackDirection;
    private bool isKnockedBack = false;
    public float knockbackDuration = 0.5f; // How long to disable following after knockback

    [Header("Hit Cooldown (player=player->enemy, enemy = enemy->player)")]
    public float playerHitCooldown = 1f; // Time between eligible hits
    private float playerLastHitTime = -1f;

    public float enemyHitCooldown = 1f; // Time between eligible hits
    private float enemyLastHitTime = -1f;

    [Header("Enemy AI")]
    public float followSpeed = 3f;
    public float viewDistance = 10f;

    public enum EnemyAIOptions { Follow, Slime }
    public EnemyAIOptions enemyAI;

    // Slime Stuff
    public float jumpForce = 7f; // Vertical distance
    public float jumpHorizontalSpeed = 3f; //Horizontal Distance
    public float jumpInterval = 2f; // Time between jumps
    private float jumpTimer; // Curernt time

    private bool wasGrounded = true;

    [Header("Animation (General)")]
    public Sprite idleRight;
    public Sprite idleLeft;
    public Sprite[] runRight; // 2 frames
    public Sprite[] runLeft;  // 2 frames

    public Vector2 rightOffset;
    public Vector2 leftOffset;

    public Transform spriteHolder;
    private SpriteRenderer sr;
    private float animationTimer = 0f;
    private int currentFrame = 0;
    public float frameRate = 0.2f; // Time per frame
    private Vector2 lastDirection = Vector2.right;

    [Header("Animation (slime-specific)")]
    public Sprite aboutToJumpRight;
    public Sprite aboutToJumpLeft;
    public Sprite inAirRight;
    public Sprite inAirLeft;
    public float aboutToJumpTime = 0.25f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = spriteHolder.GetComponent<SpriteRenderer>();

        //more slime stuff
        jumpTimer = jumpInterval;
        wasGrounded = IsGrounded();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();

        knockbackDirection = (enemy.transform.position - player.transform.position).normalized;


        // More more slime stuff to ensure it is able to jump
        jumpTimer -= Time.deltaTime;
        bool grounded = IsGrounded();
        if (grounded && !wasGrounded)
        {
            rb.linearVelocity = Vector2.zero;
            jumpTimer = jumpInterval;
        }

        // Only follow/stop if not currently knocked back
        if (!isKnockedBack)
        {
            // Checks if player is within the view distance
            float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
            if (distance < viewDistance)
            {
                switch (enemyAI)
                {
                    case EnemyAIOptions.Follow:
                        FollowPlayer();
                        break;

                    case EnemyAIOptions.Slime:
                        if (jumpTimer <= 0f && grounded)
                        {
                            SlimeFollow();
                        }
                        break;
                }
            }
            else
            {
                StopFollowing();
            }
        }

        if (enemyAI == EnemyAIOptions.Slime)
        {
            SlimeFacingAndAnimation(grounded);
        }
        else
        {
            FacingDetection();
        }

        wasGrounded = grounded;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Attack(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Attack(other);
    }

    void Attack(Collider2D other)
    {
        // Check for player hitting enemy
        if (other.CompareTag("PlayerAttack") && GlobalVariables.isDamaging)
        {
            // Check cooldown to prevent multiple hits
            if (Time.time - playerLastHitTime > playerHitCooldown)
            {
                StopFollowing();
                health--;
                ApplyKnockback(knockbackDirection, knockbackStrength);
                playerLastHitTime = Time.time;
            }
        }

        //Check for enemy hitting player
        if (other.CompareTag("Player"))
        {
            // Check cooldown to prevent multiple hits
            if (Time.time - enemyLastHitTime > enemyHitCooldown)
            {
                GlobalVariables.health--;

                // Calculate knockback direction from enemy to player

                // I dont think the player is correctly getting knocked back because of player movement,
                // but honestly its not a big deal and I can ignore it for now but eventually fixing it would be cool
                Vector2 playerKnockbackDirection = (player.transform.position - enemy.transform.position).normalized;

                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                Vector2 playerKnockback = new Vector2(playerKnockbackDirection.x * playerKnockbackStrength, playerKnockbackUpwardForce);
                playerRb.linearVelocity = Vector2.zero;
                playerRb.AddForce(playerKnockback, ForceMode2D.Impulse);

                enemyLastHitTime = Time.time;
            }
        }
    }


    public void ApplyKnockback(Vector2 direction, float force)
    {

        // Disable other movement temporarily to allow knockback to work
        isKnockedBack = true;
        Invoke(nameof(EndKnockback), knockbackDuration);

        // Stop current movement and apply knockback
        rb.linearVelocity = Vector2.zero;
        Vector2 totalKnockback = new Vector2(direction.x * force, knockbackUpwardForce);
        rb.AddForce(totalKnockback, ForceMode2D.Impulse);
    }

    private void EndKnockback()
    {
        isKnockedBack = false;
    }

    public void FollowPlayer()
    {
        Vector3 currentPosition = enemy.transform.position;
        Vector3 playerPosition = player.transform.position;

        // Calculate direction to player
        Vector2 directionToPlayer = (playerPosition - currentPosition).normalized;

        // Apply horizontal force to move towards player
        rb.AddForce(new Vector2(directionToPlayer.x * followSpeed, 0), ForceMode2D.Force);

        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.Clamp(velocity.x, -followSpeed, followSpeed);
        rb.linearVelocity = velocity;
    }

    public void StopFollowing()
    {

        // Gradually reduce horizontal velocity
        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.MoveTowards(velocity.x, 0, followSpeed * 2f * Time.deltaTime);
        rb.linearVelocity = velocity;
    }

    public void SlimeFollow()
    {
        float direction = Mathf.Sign(player.transform.position.x - enemy.transform.position.x);
        if (direction == 0f) direction = (transform.localScale.x >= 0) ? 1f : -1f;

        // Set deterministic horizontal speed for the jump (prevents sliding leftover)
        rb.linearVelocity = new Vector2(direction * jumpHorizontalSpeed, 0f);

        // Apply vertical impulse
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        // If the velocity is tiny then it is probably on the groun
        return Mathf.Abs(rb.linearVelocity.y) < 0.05f;
    }


    void FacingDetection()
    {

        float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
        if (distance < viewDistance)
        {

            if (enemy.transform.position.x < player.transform.position.x) // Moving right
            {
                if (lastDirection != Vector2.right)
                {
                    sr.sprite = runRight[0]; // show first running frame immediately
                    currentFrame = 0;
                    animationTimer = 0f;
                }

                Animate(runRight);
                lastDirection = Vector2.right;
                spriteHolder.localPosition = rightOffset;
            }
            else if (enemy.transform.position.x > player.transform.position.x) // Moving left
            {
                if (lastDirection != Vector2.left)
                {
                    sr.sprite = runLeft[0]; // show first running frame immediately
                    currentFrame = 0;
                    animationTimer = 0f;
                }

                Animate(runLeft);
                lastDirection = Vector2.left;
                spriteHolder.localPosition = leftOffset;
            }
            else // Idle
            {
                sr.sprite = (lastDirection == Vector2.right) ? idleRight : idleLeft;
                currentFrame = 0; // Reset frame index when idle
                animationTimer = 0f;
            }
        }
        else
        {
            sr.sprite = (lastDirection == Vector2.right) ? idleRight : idleLeft;
            currentFrame = 0; // Reset frame index when idle
            animationTimer = 0f;
        }
    }

    void SlimeFacingAndAnimation(bool grounded)
    {
        // Slime facing
        if (enemy.transform.position.x < player.transform.position.x)
            lastDirection = Vector2.right;
        else if (enemy.transform.position.x > player.transform.position.x)
            lastDirection = Vector2.left;

        // please work
        if (grounded)
        {
            if (jumpTimer <= aboutToJumpTime)
            {
                // about to jump
                sr.sprite = (lastDirection == Vector2.right) ? aboutToJumpRight : aboutToJumpLeft;
            }
            else
            {
                // idle 
                sr.sprite = (lastDirection == Vector2.right) ? idleRight : idleLeft;
            }

            // reset animation
            currentFrame = 0;
            animationTimer = 0f;
        }
        else
        {
            // in air
            sr.sprite = (lastDirection == Vector2.right) ? inAirRight : inAirLeft;
        }

        spriteHolder.localPosition = (lastDirection == Vector2.right) ? rightOffset : leftOffset;
    }

    void Animate(Sprite[] frames)
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= frameRate)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % frames.Length;
            sr.sprite = frames[currentFrame];
        }
    }
    
    

}
