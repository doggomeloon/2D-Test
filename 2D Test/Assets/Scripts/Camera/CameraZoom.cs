using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cam;               // Reference to your main camera
    public float zoomSpeed = 5f;     // Speed of zoom
    public float minZoom = 2f;       // Minimum zoom size
    public float maxZoom = 10f;      // Maximum zoom size

    void Update()
    {
        // Zoom with mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            float newSize = cam.orthographicSize - scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }

        // Optional: Zoom in/out with keyboard
        if (Input.GetKey(KeyCode.Equals)) // press '=' key
        {
            cam.orthographicSize = Mathf.Max(minZoom, cam.orthographicSize - zoomSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Minus)) // press '-' key
        {
            cam.orthographicSize = Mathf.Min(maxZoom, cam.orthographicSize + zoomSpeed * Time.deltaTime);
        }
    }
}
