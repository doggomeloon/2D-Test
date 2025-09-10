using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections;

public class TextTrigger : MonoBehaviour
{
    public GameObject textBox; // Text box
    public string message;     // the text for the box
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = textBox.GetComponentInChildren<TextMeshProUGUI>();
        textBox.SetActive(false); // hide at start
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        textComponent.text = "";
        {
            textBox.SetActive(true);
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        
        for (int i = 0; i < message.Length; i++)
        {
            textComponent.text += message[i];
            yield return new WaitForSeconds(0.03f);
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
