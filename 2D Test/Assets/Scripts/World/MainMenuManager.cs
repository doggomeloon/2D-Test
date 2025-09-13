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



    public void PlayGame()
    {
        // When they click the button it should make it the next scene // PLEASE KEEP SCENE ORDER CORRECT
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!"); 
        Application.Quit(); 
    }
}
