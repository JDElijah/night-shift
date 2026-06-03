using UnityEngine;
using UnityEngine.SceneManagement;

/*
    MainMenu.cs handles main menu button actions, such as starting the game
    and quitting the application. 
 */

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // All of these examples loads "SampleScene" will rename later. 
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked"); 
        Application.Quit();
    }
}
