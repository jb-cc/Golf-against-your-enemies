using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager == null)
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void TogglePauseMenu()
    {
        _gameManager.TogglePauseMenu();
    }
    
    public void MainMenu()
    {
        _gameManager.MainMenu();
    }

    public void QuitGame()
    {
        _gameManager.QuitGame();
    }
}
