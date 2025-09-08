using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Player
    public float smoothSpeed = 0.125f; // Lower = more lag/smoothing
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Optional Level Bounds")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private void LateUpdate()
    {
        if (target == null) return;

        // Desired position with offset
        Vector3 desiredPosition = target.position + offset;

        // Smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamp inside bounds if enabled
        if (useBounds)
        {
            float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
            smoothedPosition = new Vector3(clampedX, clampedY, smoothedPosition.z);
        }

        transform.position = smoothedPosition;
    }
}
