using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button StartGameButton;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        StartGameButton.onClick.AddListener(OnStartGameButtonClick);
    }

    private void OnStartGameButtonClick()
    {
        gameManager.StartGame();
        gameObject.SetActive(false);
    }
}
