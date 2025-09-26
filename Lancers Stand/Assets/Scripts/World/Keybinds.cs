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
    public GameObject attackIcon;
    public GameObject interactIcon;
    public GameObject zoomOutIcon;
    public GameObject zoomInIcon;

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
                case "attack": focusedImage = attackIcon.GetComponent<Image>(); break;
                case "interact": focusedImage = interactIcon.GetComponent<Image>(); break;
                case "zoomIn": focusedImage = zoomInIcon.GetComponent<Image>(); break;
                case "zoomOut": focusedImage = zoomOutIcon.GetComponent<Image>(); break;
            }

            Sprite emptySprite = Resources.Load<Sprite>("Sprites/LetterIcons/Empty");
            focusedImage.sprite = emptySprite;
            RectTransform rt = focusedImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(emptySprite.texture.width*2, emptySprite.texture.height*2);
        }
    }

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
            case "attack":
                GlobalVariables.attackKey = newKey;
                break;
            case "interact":
                GlobalVariables.interactKey = newKey;
                break;
            case "zoomIn":
                GlobalVariables.interactKey = newKey;
                break;
            case "zoomOut":
                GlobalVariables.interactKey = newKey;
                break;
        }
        Sprite newSprite = Resources.Load<Sprite>("Sprites/LetterIcons/" + newKey.ToString());
        focusedImage.sprite = newSprite;
        RectTransform rt = focusedImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(newSprite.texture.width*2, newSprite.texture.height*2);
    }


    private void CancelEditing()
    {
        GlobalVariables.currentlyEditing = "";
        GlobalVariables.focusLocked = false;
        focusedImage = null;
    }

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

