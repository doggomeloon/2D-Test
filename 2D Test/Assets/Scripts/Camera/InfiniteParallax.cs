using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor; // 0 = static, 1 = camera speed
    private Vector3 lastCameraPosition;

    private float spriteWidth;
    private Transform[] children;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        // Get width of sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * -parallaxFactor, delta.y * -parallaxFactor, 0);
        lastCameraPosition = cameraTransform.position;

        // Reposition for infinite scrolling
        if (cameraTransform.position.x > transform.position.x + spriteWidth)
        {
            transform.position += new Vector3(spriteWidth*2, 0, 0);
        }
        else if (cameraTransform.position.x < transform.position.x - spriteWidth)
        {
            transform.position -= new Vector3(spriteWidth*2, 0, 0);
        }
    }
}
