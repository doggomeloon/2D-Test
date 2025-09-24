using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public int health = 5;

    [Header("Objects")]
    public GameObject player;

    private Collider2D attackCollider;
    public GameObject enemy;
    private Collider2D collide;
    public TextMeshProUGUI healthText;

    [Header("Knockback")]
    public float knockbackStrength = 5f;
    public float knockbackUpwardForce = 5f; // Add vertical jump to knockback
    private Vector2 knockbackDirection;
    private bool isKnockedBack = false;
    public float knockbackDuration = 0.5f; // How long to disable following after knockback

    [Header("Hit Cooldown")]
    public float hitCooldown = 1f; // Half second cooldown between hits
    private float lastHitTime = -1f;

    [Header("Enemy AI")]
    public float followSpeed = 3f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collide = enemy.GetComponent<Collider2D>();
        attackCollider = player.GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();

        knockbackDirection = (enemy.transform.position - player.transform.position).normalized;

        // Only follow/stop if not currently knocked back
        if (!isKnockedBack)
        {
            // Checks if player is less than 10 units away
            float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
            if (distance < 10f)
            {
                FollowPlayer();
            }
            else
            {
                StopFollowing();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            // Check cooldown to prevent multiple hits
            if (Time.time - lastHitTime > hitCooldown)
            {
                StopFollowing();
                health--;
                ApplyKnockback(knockbackDirection, knockbackStrength);
                lastHitTime = Time.time;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // While they are overlapping
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Once they leave
    }


    public void ApplyKnockback(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        // Disable following temporarily to allow knockback to work
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        // Gradually reduce horizontal velocity to make the enemy stop smoothly
        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.MoveTowards(velocity.x, 0, followSpeed * 2f * Time.deltaTime);
        rb.linearVelocity = velocity;
    }

}
