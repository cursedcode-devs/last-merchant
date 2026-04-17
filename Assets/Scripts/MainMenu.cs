using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {

        SceneManager.LoadScene("Michal NAJNOWSZE");
    }

    public void QuitGame()
    {

        Application.Quit();
    }
}