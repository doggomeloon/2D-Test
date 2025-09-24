using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite idleRight;
    public Sprite idleLeft;
    public Sprite[] runRight; // 2 frames
    public Sprite[] runLeft;  // 2 frames

    public Vector2 rightOffset; 
    public Vector2 leftOffset;

    public Transform spriteHolder;
    public Transform attackBox;
    private SpriteRenderer sr;
    private float animationTimer = 0f;
    private int currentFrame = 0;
    public float frameRate = 0.2f; // Time per frame
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        sr = spriteHolder.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!GlobalVariables.focusLocked) {
            float moveX = 0f;

            if (Input.GetKey(GlobalVariables.leftKey))  
            {
                moveX = -1f;
            }
            else if (Input.GetKey(GlobalVariables.rightKey)) 
            {   
                moveX = 1f;
            }
            Vector2 moveDir = new Vector2(moveX, 0);

            if (moveDir.x > 0) // Moving right
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
                attackBox.localPosition = new Vector3(0.0618f, -0.067f, 0f);
            }
            else if (moveDir.x < 0) // Moving left
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
                attackBox.localPosition = new Vector3(-0.267f, -0.067f, 0f);
            }
            else // Idle
            {
                sr.sprite = (lastDirection == Vector2.right) ? idleRight : idleLeft;
                currentFrame = 0; // Reset frame index when idle
                animationTimer = 0f;
            }
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
