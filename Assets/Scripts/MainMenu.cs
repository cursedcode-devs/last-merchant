using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;

    [Header("How To Play Slides")]
    [SerializeField] private GameObject howToPlayPanel1;
    [SerializeField] private GameObject howToPlayPanel2;
    [SerializeField] private GameObject howToPlayPanel3;
    [SerializeField] private GameObject howToPlayPanel4;

    public void StartGame()
    {
        SceneManager.LoadScene("FinalRelease");
    }

    public void HowToPlay()
    {
        SetMenuVisible(false);
        ShowSlide(1);
    }

    public void NextSlide1()
    {
        ShowSlide(2);
    }

    public void NextSlide2()
    {
        ShowSlide(3);
    }

    public void NextSlide3()
    {
        ShowSlide(4);
    }

    public void CloseHowToPlay()
    {
        HideAllSlides();
        SetMenuVisible(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // ---- Helpers ----

    private void SetMenuVisible(bool visible)
    {
        if (menuPanel != null)
            menuPanel.SetActive(visible);
    }

    private void HideAllSlides()
    {
        if (howToPlayPanel1 != null) howToPlayPanel1.SetActive(false);
        if (howToPlayPanel2 != null) howToPlayPanel2.SetActive(false);
        if (howToPlayPanel3 != null) howToPlayPanel3.SetActive(false);
        if (howToPlayPanel4 != null) howToPlayPanel4.SetActive(false);
    }

    private void ShowSlide(int index)
    {
        HideAllSlides();
        switch (index)
        {
            case 1: if (howToPlayPanel1 != null) howToPlayPanel1.SetActive(true); break;
            case 2: if (howToPlayPanel2 != null) howToPlayPanel2.SetActive(true); break;
            case 3: if (howToPlayPanel3 != null) howToPlayPanel3.SetActive(true); break;
            case 4: if (howToPlayPanel4 != null) howToPlayPanel4.SetActive(true); break;
        }
    }
}