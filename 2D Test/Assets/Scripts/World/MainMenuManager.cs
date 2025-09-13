using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
    }

    public void QuitGame()
    {
        Debug.Log("Quit!"); 
        Application.Quit(); 
    }
}
