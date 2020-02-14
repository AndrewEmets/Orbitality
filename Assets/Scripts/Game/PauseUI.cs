using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button exitButton, pauseButton, continueButton;
    [SerializeField] private GameObject pausePanel;
    
    private void Start()
    {
        exitButton.onClick.AddListener(ExitToMainMenu);
        pauseButton.onClick.AddListener(PauseButtonClick);
        continueButton.onClick.AddListener(ContinueButtonClick);
    }

    private void ContinueButtonClick()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void PauseButtonClick()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    private void ExitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }
}
