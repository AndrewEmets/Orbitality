using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject gameOverPanel, losePanel, winPanel;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        gameManager.GameFinished += GameFinished;
        
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void GameFinished(GameManager.GameResult result)
    {
        if (result == GameManager.GameResult.Win)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
        
        gameOverPanel.SetActive(true);
    }
}