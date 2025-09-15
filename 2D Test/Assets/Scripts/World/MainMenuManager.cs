using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject playButton;
    public GameObject settingsButton;
    public GameObject creditsButton;
    public GameObject settings;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GlobalVariables.currentScene == "MainMenu")
            {
                QuitGame();
            }
            else
            {
                DisableSettings();
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void EnableSettings()
    {
        GlobalVariables.currentScene = "Settings";
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        creditsButton.SetActive(false);

        settings.SetActive(true);
    }

    public void DisableSettings()
    {
        GlobalVariables.currentScene = "MainMenu";
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        creditsButton.SetActive(true);

        settings.SetActive(false);
    }
}
