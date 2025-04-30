using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject helpPanel;
    public GameObject gameplayUIRoot; // ✅ Drag your Gameplay UI parent here

    [Header("Buttons")]
    public Button resumeButton;
    public Button helpButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        helpPanel.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        helpButton.onClick.AddListener(ShowHelp);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        pausePanel.SetActive(true);
        helpPanel.SetActive(false);

        if (gameplayUIRoot != null)
            gameplayUIRoot.SetActive(false); // ✅ Hide all gameplay UI
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        pausePanel.SetActive(false);

        if (gameplayUIRoot != null)
            gameplayUIRoot.SetActive(true); // ✅ Reactivate gameplay UI
    }

    void ShowHelp()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit button clicked. Application would close in build.");
    }
}
