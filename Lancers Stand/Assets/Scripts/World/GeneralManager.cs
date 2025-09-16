using UnityEngine;
using UnityEngine.SceneManagement;

public class generalManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] interactionBlocks = GameObject.FindGameObjectsWithTag("InteractionBlocks");

        foreach (GameObject block in interactionBlocks)
        {
            // Example: change their SpriteRenderer color
            SpriteRenderer sr = block.GetComponent<SpriteRenderer>();

            sr.sprite = Resources.Load<Sprite>("Sprites/LetterIcons/" + GlobalVariables.interactKey.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + -1);
        }
    }
}
