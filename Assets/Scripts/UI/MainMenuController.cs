using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton, loadButton, exitButton;
    [SerializeField] private Button decreasePlayers, increasePlayers;
    [SerializeField] private Text playersCountLabel;

    private int playersCount = 2;
    
    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        loadButton.onClick.AddListener(OnLoadButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        
        decreasePlayers.onClick.AddListener(OnDecreasePlayersClick);
        increasePlayers.onClick.AddListener(OnIncreasePlayersClick);
    }

    #region Players count
    
    private void ChangePlayersCount(int i)
    {
        playersCount += i;

        playersCount = Mathf.Clamp(playersCount, 2, 6);
        playersCountLabel.text = playersCount.ToString();
    }

    private void OnIncreasePlayersClick()
    {
        ChangePlayersCount(1);
    }

    private void OnDecreasePlayersClick()
    {
        ChangePlayersCount(-1);
    }
    
    #endregion

    private void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;

        void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var gameManager = FindObjectOfType<GameManager>();
            gameManager.StartGame(new StartGameParameters
            {
                playersCount = playersCount
            });
            
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }
    }

    private void OnLoadButtonClick()
    {
        
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}
