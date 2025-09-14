using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite idleRight;
    public Sprite idleLeft;
    public Sprite[] runRight; // 2 frames
    public Sprite[] runLeft;  // 2 frames

    private SpriteRenderer sr;
    private float animationTimer = 0f;
    private int currentFrame = 0;
    public float frameRate = 0.2f; // Time per frame
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 moveDir = new Vector2(moveX, 0);

        if (moveDir.x > 0) // Moving right
        {
            Animate(runRight);
            lastDirection = Vector2.right;
        }
        else if (moveDir.x < 0) // Moving left
        {
            Animate(runLeft);
            lastDirection = Vector2.left;
        }
        else // Idle
        {
            sr.sprite = (lastDirection == Vector2.right) ? idleRight : idleLeft;
            currentFrame = 0; // Reset frame index when idle
            animationTimer = 0f;
        }
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
