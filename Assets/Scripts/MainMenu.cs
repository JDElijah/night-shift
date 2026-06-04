using UnityEngine;
using UnityEngine.SceneManagement;

/*
    MainMenu.cs handles main menu button actions, such as starting the game
    and quitting the application. 
 */

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string level01 = "Level01";
   public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // All of these examples loads "SampleScene" will rename later. 
        SceneManager.LoadScene(level01);
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked"); 
        Application.Quit();
    }
}
