using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor; // 0 = static, 1 = camera speed
    private Vector3 lastCameraPosition;

    private float spriteWidth;
    private Transform[] children;

    // Thank you stack overflow for making this work

    void Start()
    {

        lastCameraPosition = cameraTransform.position;

        // Get width of sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * -parallaxFactor, delta.y * -parallaxFactor, 0);
        lastCameraPosition = cameraTransform.position; // Moves the backgrounds based on the camera's current position

        // Reposition for infinite scrolling
        // There are 3 sets of each background, it moves, the furthest background to the front to make it look infinte
        // Set Main Camera size to 50 in inspector while playing to see it for yourself its pretty cool im proud of it
        if (cameraTransform.position.x > transform.position.x + spriteWidth)
        {
            transform.position += new Vector3(spriteWidth * 2, 0, 0);
        }
        else if (cameraTransform.position.x < transform.position.x - spriteWidth)
        {
            transform.position -= new Vector3(spriteWidth * 2, 0, 0);
        }
    }
}
