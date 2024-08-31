using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private GameManager _gameManager;
    void Start()
    {
        if (_gameManager == null)
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    public void StartGame()
    {
        _gameManager.StartGame();
    }
    
    public void StartPractice()
    {
        _gameManager.StartPractice();
    }
    
    public void QuitGame()
    {
        _gameManager.QuitGame();
    }
}
