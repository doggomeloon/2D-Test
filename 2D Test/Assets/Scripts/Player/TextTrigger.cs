using UnityEngine;
using TMPro;
using System.Collections;
using Unity.AppUI.UI;
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
    private bool isVisible = false;




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
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDialogue();
                portraitPanel.sprite = portrait;
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

    private IEnumerator TypeText()
    {
        textComponent.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            textComponent.text += message[i];
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void ToggleDialogue()
    {
        isVisible = !isVisible;
        dialogueBox.gameObject.SetActive(isVisible);
        StopAllCoroutines();
        StartCoroutine(Zoom(isVisible ? Vector3.one : Vector3.zero));
        StartCoroutine(TypeText());
    }
    
    private System.Collections.IEnumerator Zoom(Vector3 targetScale)
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
