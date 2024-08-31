using UnityEngine;

public class LevelSelectionMenu : MonoBehaviour
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager == null)
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SelectIslandsLevel()
    {
        _gameManager.StartIslandsLevel();
    }
}
