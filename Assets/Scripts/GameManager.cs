using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private Board board;

    public void StartGame()
    {
        board.TileGenerate();
        board.Init();
        gameOverPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        board.Init();
        gameOverPanel.gameObject.SetActive(false);
    }


    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
