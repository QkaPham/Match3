using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnNewGameButtonClick()
    {
        gameManager.RestartGame();
    }

    private void OnQuitButtonClick()
    {
        gameManager.Quit();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
