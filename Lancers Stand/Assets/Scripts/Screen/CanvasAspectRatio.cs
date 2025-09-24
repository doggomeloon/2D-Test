using UnityEngine;
using UnityEngine.UI;

public class CanvasAspectRatio : MonoBehaviour
{
    void Start()
    {
        AdjustCanvasScalers();
    }

    void AdjustCanvasScalers()
    {
        float aspect = (float)Screen.width / Screen.height;

        // Find all CanvasScaler components in this scene
        CanvasScaler[] scalers = FindObjectsByType<CanvasScaler>(FindObjectsSortMode.None);

        foreach (CanvasScaler scaler in scalers)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Keep 16:9 baseline
            scaler.referenceResolution = new Vector2(2560, 1440);

            // Im specifically allowing 16:10 bc my laptop is,
            // but most computers have 16:9 ratio
            if (Mathf.Abs(aspect - (16f / 10f)) < 0.01f)
            {
                // On 16:10 screen
                scaler.matchWidthOrHeight = 0f; // match width
            }
            else
            {
                // On 16:9 or others
                scaler.matchWidthOrHeight = 0.5f;
            }
        }
    }
}
