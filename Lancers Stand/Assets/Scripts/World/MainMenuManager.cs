using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenu : MonoBehaviour
{

    public GameObject playButton;
    public GameObject settingsButton;
    public GameObject creditsButton;
    public GameObject settings;

    public TMP_Text quitTag;

    public void Start()
    {
        settings.SetActive(false);
        
    }
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
        quitTag.text = "Press Escape to Return to Menu!";
    }

    public void DisableSettings()
    {
        GlobalVariables.currentScene = "MainMenu";
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        creditsButton.SetActive(true);

        settings.SetActive(false);
        quitTag.text = "Press Escape to Quit!";
    }

    
}
