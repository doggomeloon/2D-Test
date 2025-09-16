using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class ResolutionDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown; // Assign your TMP Dropdown in the Inspector
    private List<(int width, int height, int hz)> uniqueResolutions;

    void Start()
    {

        // Get all resolutions from the system
        Resolution[] allResolutions = Screen.resolutions;

        // Convert to (width, height, roundedHz) and filter only 16:9 or 16:10
        uniqueResolutions = allResolutions
            .Select(res => (res.width, res.height, Mathf.RoundToInt((float)res.refreshRateRatio.value)))
            .Where(res =>
            {
                float aspect = (float)res.width / res.height;
                return Mathf.Abs(aspect - (16f / 9f)) < 0.01f ||
                       Mathf.Abs(aspect - (16f / 10f)) < 0.01f;
            })
            .Distinct() // remove duplicates (same width, height, Hz)
            .OrderByDescending(res => res.width * res.height) // higher resolution first
            .ThenByDescending(res => res.Item3)               // then higher Hz first
            .ToList();

        // Clear old dropdown options
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            var res = uniqueResolutions[i];
            string option = $"{res.width} x {res.height} @{res.hz}Hz";
            options.Add(option);

            // Match with current resolution
            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height &&
                res.hz == Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value))
            {
                currentResolutionIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();

        // Apply resolution on selection
        dropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int index)
    {
        var chosen = uniqueResolutions[index];

        // Wrap refresh rate into RefreshRate struct
        var refreshRate = new RefreshRate { numerator = (uint)chosen.hz, denominator = (uint)1 };
        Screen.SetResolution(chosen.width, chosen.height, Screen.fullScreenMode, refreshRate);
        Debug.Log($"Resolution set to: {chosen.width} x {chosen.height} @{chosen.hz}Hz");
    }
}
