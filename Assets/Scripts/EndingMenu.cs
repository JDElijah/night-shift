using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
    }


    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame.");

        Application.Quit();
    }
}
