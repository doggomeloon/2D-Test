using UnityEngine;
using TMPro; // if you use TextMeshPro

public class TextTrigger : MonoBehaviour
{
    public GameObject textBox; // assign your UI panel here
    public string message;     // the message for this zone
    private TextMeshProUGUI textComponent;

    void Start()
    {
        if (textBox != null)
        {
            textComponent = textBox.GetComponentInChildren<TextMeshProUGUI>();
            textBox.SetActive(false); // hide at start
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textBox != null)
            {
                textComponent.text = message;
                textBox.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textBox != null)
            {
                textBox.SetActive(false);
            }
        }
    }
}
