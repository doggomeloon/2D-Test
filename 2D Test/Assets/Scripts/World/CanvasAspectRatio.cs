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

            if (Mathf.Abs(aspect - (16f / 10f)) < 0.01f)
            {
                // On 16:10 screen (like 2560x1600) → stretch to fill
                scaler.matchWidthOrHeight = 0f; // match width
            }
            else
            {
                // On 16:9 or others → balanced scaling
                scaler.matchWidthOrHeight = 0.5f;
            }
        }
    }
}
