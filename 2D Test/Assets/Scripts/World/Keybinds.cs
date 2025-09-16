using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Keybinds : MonoBehaviour
{
    // UI references
    public GameObject leftIcon;
    public GameObject rightIcon;
    public GameObject jumpIcon;
    public GameObject sprintIcon;
    public GameObject interactIcon;

    private Image focusedImage;   // highlight the currently editing key

    private void Update()
    {
        // Only listen for key input if editing a bind
        if (!string.IsNullOrEmpty(GlobalVariables.currentlyEditing))
        {
            if (Input.anyKeyDown)
            {
                // Handle cancel
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelEditing();
                    return;
                }

                // Detect which key was pressed
                KeyCode pressedKey = DetectPressedKey();
                Debug.Log(pressedKey);

                // Check against eligible keys
                if (pressedKey != KeyCode.None && GlobalVariables.eligibleKeys.Contains(pressedKey.ToString()))
                {
                    ApplyKeybind(GlobalVariables.currentlyEditing, pressedKey);
                    CancelEditing(); // exit editing after success
                }
            }
        }
    }


    /// <summary>
    /// Called when a UI button is pressed
    /// </summary>
    public void EditKeybind(string key)
    {
        if (GlobalVariables.currentlyEditing == key)
        {
            // If clicked again, cancel editing
            CancelEditing();
        }
        else
        {
            // Switch to new key
            GlobalVariables.currentlyEditing = key;
            GlobalVariables.focusLocked = true;

            // Highlight the correct icon
            switch (key)
            {
                case "left": focusedImage = leftIcon.GetComponent<Image>(); break;
                case "right": focusedImage = rightIcon.GetComponent<Image>(); break;
                case "jump": focusedImage = jumpIcon.GetComponent<Image>(); break;
                case "sprint": focusedImage = sprintIcon.GetComponent<Image>(); break;
                case "interact": focusedImage = interactIcon.GetComponent<Image>(); break;
            }

            Sprite emptySprite = Resources.Load<Sprite>("Sprites/LetterIcons/Empty");
            focusedImage.sprite = emptySprite;
            RectTransform rt = focusedImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(emptySprite.texture.width*2, emptySprite.texture.height*2);
        }
    }

    /// <summary>
    /// Assigns the new key to the action
    /// </summary>
    private void ApplyKeybind(string action, KeyCode newKey)
    {
        Debug.Log($"Assigned {newKey} to {action}");

        switch (action)
        {
            case "left":
                GlobalVariables.leftKey = newKey;
                break;
            case "right":
                GlobalVariables.rightKey = newKey;
                break;
            case "jump":
                GlobalVariables.jumpKey = newKey;
                break;
            case "sprint":
                GlobalVariables.sprintKey = newKey;
                break;
            case "interact":
                GlobalVariables.interactKey = newKey;
                break;
        }
        Sprite newSprite = Resources.Load<Sprite>("Sprites/LetterIcons/" + newKey.ToString());
        focusedImage.sprite = newSprite;
        RectTransform rt = focusedImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(newSprite.texture.width*2, newSprite.texture.height*2);
    }

    /// <summary>
    /// Reset editing state
    /// </summary>
    private void CancelEditing()
    {
        GlobalVariables.currentlyEditing = "";
        GlobalVariables.focusLocked = false;
        focusedImage = null;
    }

    /// <summary>
    /// Detects which KeyCode was pressed
    /// </summary>
    private KeyCode DetectPressedKey()
    {
        foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(code))
            {
                return code;
            }
        }
        return KeyCode.None;
    }
}

