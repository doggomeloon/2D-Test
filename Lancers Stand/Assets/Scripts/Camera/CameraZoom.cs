using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cam; // Put main camera here, idk if this should ever really change
    public float zoomSpeed = 5f;// Speed of zoom
    public float minZoom = 2f;// Minimum zoom size, i believe 2.75 is best
    public float maxZoom = 10f; // Maximum zoom size, 10 is pretty good

    //TODO: If the camera ever changes, make sure camera zoom is DISABLED

    void Update()
    {
        // Zoom with mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            float newSize = cam.orthographicSize - scroll * zoomSpeed; // Changes the size of the camera based on scroll wheel
            cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        } 

        // Zooming in and out w keyboard
        if (Input.GetKey(KeyCode.Equals)) // '=' key
        {
            //Same function as before but with '=' and '-' keys
            cam.orthographicSize = Mathf.Max(minZoom, cam.orthographicSize - zoomSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Minus)) // '-' key
        {
            cam.orthographicSize = Mathf.Min(maxZoom, cam.orthographicSize + zoomSpeed * Time.deltaTime);
        }
    }
}
