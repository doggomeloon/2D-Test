using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Should generally be the player but I think if we do cutscenes then it can be smth else
    public float smoothSpeed = 0.125f; // Lower = more lag/smoothing
    public Vector3 offset = new Vector3(0f, 0f, -10f); // -10 by default bc the camera needs to be behind  everything in 3d space

    [Header("Level Bounds")] // For future use if needed for a level idk if we will use it though
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private void LateUpdate()
    {

        // Desired position with offset
        Vector3 targetPosition = target.position + offset;

        // Linearly interpolates to target should be smooth most of the time i think
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed); 

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
