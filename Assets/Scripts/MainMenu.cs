using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("FinalRelease");
    }

    public void HowToPlay()
    {
        Debug.Log("How to play clicked!");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}