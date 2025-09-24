using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{

    public int health = 5;
    public GameObject player;

    private Collider2D attackCollider;
    public GameObject enemy;
    private Collider2D collide;

    public TextMeshProUGUI healthText;

    private Vector2 knockbackDirection;
    public float knockbackStrength = 10f;

    // Add cooldown to prevent multiple hits
    private float lastHitTime = -1f;
    public float hitCooldown = 1f; // Half second cooldown between hits


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

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            // Check cooldown to prevent multiple hits
            if (Time.time - lastHitTime > hitCooldown)
            {
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
        
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

}
