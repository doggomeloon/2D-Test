using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public RectTransform dialogueBox;
    public float zoomSpeed = 8f;
    private bool isVisible = false;


    void Start()
    {
        isVisible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // test toggle
        {
            ToggleDialogue();
        }
    }

    public void ToggleDialogue()
    {
        isVisible = !isVisible;
        StopAllCoroutines();
        StartCoroutine(Zoom(isVisible ? Vector3.one : Vector3.zero));
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
