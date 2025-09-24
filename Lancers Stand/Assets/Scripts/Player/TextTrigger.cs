using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour
{
    public RectTransform dialogueBox; // Dialogue box
    public GameObject textBox;
    public GameObject interactButton;
    public string message; // Text for the box
    private TextMeshProUGUI textComponent; // TMP Text
    public Image portraitPanel; // Path to Image

    public Sprite portrait; // Character Image

    private bool isInArea = false;
    public float zoomSpeed = 8f;

    public float textSpeed = 0.03f;
    private bool isVisible = false;

    public bool requiresFocus = false; //If true, the player will be unable to move while interacted




    void Start()
    {
        textComponent = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogueBox.gameObject.SetActive(false); // hide at start
        interactButton.SetActive(false);
    }

    void Update()
    {
        if (isInArea)
        {
            if (Input.GetKeyDown(GlobalVariables.interactKey))
            {
                textComponent.text = "";
                portraitPanel.sprite = portrait;
                ToggleDialogue();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInArea = true;
            interactButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInArea = false;
            if (isVisible) { ToggleDialogue(); }
            interactButton.SetActive(false);
        }
    }

    public void ToggleDialogue()
    {
        isVisible = !isVisible; // Toggles visibility of dialogue
        if (requiresFocus) { GlobalVariables.focusLocked = !GlobalVariables.focusLocked; } // Forces the player to stop moving
        dialogueBox.gameObject.SetActive(isVisible); // Sets visiblity of dialogue box
        StopAllCoroutines(); //Stops all previous coroutines
        StartCoroutine(Zoom(isVisible ? Vector3.one : Vector3.zero)); // Starts zooming based on current position
        if (isVisible)
        {
            StartCoroutine(TypeText()); // Types out the text slowly at 'textSpeed' speed
        }
        else
        {
            textComponent.text = "";
        }
        
        
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            textComponent.text += message[i];
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private IEnumerator Zoom(Vector3 targetScale)
    {
        while (dialogueBox.localScale != targetScale)
        {
            dialogueBox.localScale = Vector3.Lerp(
                dialogueBox.localScale,
                targetScale,
                Time.deltaTime * zoomSpeed
            );
            yield return null;
        }
    }
}
