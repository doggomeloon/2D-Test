using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public RectTransform dialogueBox;
    public float slideSpeed = 500f;

    private bool isVisible = false;

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
        StartCoroutine(Slide(isVisible ? 0 : -dialogueBox.rect.height));
    }

    private System.Collections.IEnumerator Slide(float targetY)
    {
        while (Mathf.Abs(dialogueBox.anchoredPosition.y - targetY) > 0.1f)
        {
            dialogueBox.anchoredPosition = Vector2.MoveTowards(
                dialogueBox.anchoredPosition,
                new Vector2(0, targetY),
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}
