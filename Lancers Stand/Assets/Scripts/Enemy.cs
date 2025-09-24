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

    [Header("Knockback (player=player->enemy, enemy = enemy->player)")]
    public float knockbackStrength = 5f;
    public float knockbackUpwardForce = 5f; // Add vertical jump to knockback
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
            // Checks if player is within the view distance
            float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
            if (distance < viewDistance)
            {
                switch (enemyAI)
                {
                    case EnemyAIOptions.Follow:
                        FollowPlayer();
                        break;
                }
            }
            else
            {
                switch (enemyAI)
                {
                    case EnemyAIOptions.Follow:
                        StopFollowing();
                        break;
                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check for player hitting enemy
        if (other.CompareTag("PlayerAttack"))
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
