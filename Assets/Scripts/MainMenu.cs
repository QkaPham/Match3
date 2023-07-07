using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        StartGameButton.onClick.AddListener(OnStartGameButtonClick);
        QuitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnStartGameButtonClick()
    {
        gameManager.StartGame();
        gameObject.SetActive(false);
    }

    private void OnQuitButtonClick()
    {
        gameManager.Quit();
    }
}
