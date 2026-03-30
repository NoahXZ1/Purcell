using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject TutorialPanel;
    
    public void StartGame(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OpenSettings()
    {
        //enter the tutorial panel
        if (MainMenuPanel != null) 
        {
            MainMenuPanel.SetActive(false);
        }

        if (TutorialPanel != null) 
        {
            TutorialPanel.SetActive(true);
        }
    }

     public void BackToMainMenu()
    {
        //back to start menu from tutorial menu
        if (TutorialPanel != null) 
        {
            TutorialPanel.SetActive(false);
        }

        if (MainMenuPanel != null) 
        {
            MainMenuPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
