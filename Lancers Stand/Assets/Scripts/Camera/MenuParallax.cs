using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float scrollSpeed = 1f; // Foreground = higher, background = lower

    public Transform cameraTransform;
    private float spriteWidth;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Get width of sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // Move background left
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= cameraTransform.position.x - spriteWidth) {
            transform.position += new Vector3(spriteWidth * 3, 0, 0);
        }
    }
}
