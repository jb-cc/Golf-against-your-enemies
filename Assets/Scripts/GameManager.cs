using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    private EventSystem _eventSystem;
    private GameObject _canvas;
    private GameObject _startMenu;
    private GameObject _pauseMenu;
    private GameObject _strokeCounter;
    private GameObject _scoreboard;
    private bool _gamePaused = false;
    [SerializeField] private TextMeshProUGUI[] scoreboardTexts;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI hitCounterText;
    public int CurrentLevel { get; private set; } = 1;
    public int TotalLevels { get; private set; } = 13;
    public bool GameWon { get; private set; } = false;
    public Dictionary<string, int> HitCounter { private set; get; } = new Dictionary<string, int>();
    public int maxHitsPerLevel = 12;
    void Awake()
    {
        // Singleton pattern because my menus got fucked up
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (_eventSystem == null)
            _eventSystem = FindObjectOfType<EventSystem>();
 
        if (_canvas == null)
            _canvas = GameObject.Find("Canvas");
        
        if (_startMenu == null)
            _startMenu = _canvas.transform.Find("StartMenu").gameObject;

        if (_pauseMenu == null)
            _pauseMenu = _canvas.transform.Find("PauseMenu").gameObject;

        if (_strokeCounter == null)
            _strokeCounter = _canvas.transform.Find("StrokeCounter").gameObject;

        if (_scoreboard == null)
            _scoreboard = _canvas.transform.Find("Scoreboard").gameObject;

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_eventSystem.gameObject);
        DontDestroyOnLoad(_canvas.gameObject);
        
        
        // Initialize the hit counter for each level to 0
        for (int i=1; i<=TotalLevels; i++)
        {
            HitCounter.Add("Level " + i, 0);
        }
        RefreshScoreboard();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "START")
        {
            SceneManager.LoadScene("Menu");
            _startMenu.SetActive(true);
            _scoreboard.SetActive(false);
            _pauseMenu.SetActive(false);
            _strokeCounter.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.LogError("GameManager should be in the START scene, but in scene: " + SceneManager.GetActiveScene().name);
        }
    }

    private void Update()
    {
        // Show the scoreboard when the tab key is pressed
        if (Input.GetKey(KeyCode.Tab))
        {
            _scoreboard.SetActive(true); 
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            _scoreboard.SetActive(false);
        }
        HandleEscapeKey();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("1");
        _startMenu.SetActive(false);
        _strokeCounter.SetActive(true);
        _scoreboard.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartPractice()
    {
        SceneManager.LoadScene("Practice");
        _startMenu.SetActive(false);
        _strokeCounter.SetActive(true);
        _scoreboard.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Show the pause menu when the escape key is pressed
    public void HandleEscapeKey()
    {
        if(SceneManager.GetActiveScene().name != ("Menu") && !GameWon)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }
    }
    
    public void TogglePauseMenu()
    {
        Debug.Log(this);
        Debug.Log("TogglePauseMenu called from instance: " + GetInstanceID());
        if (_gamePaused)
        {
            _pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            _gamePaused = false;
        }
        else
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            _gamePaused = true;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
        _startMenu.SetActive(true);
        _pauseMenu.SetActive(false);
        _strokeCounter.SetActive(false);
        _scoreboard.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextLevel()
    {
        RefreshScoreboard();
        if (GameWon)
        {
            Debug.Log("In NextLevel(): Game Won, No more levels to load");
            return;
        }
        if (CurrentLevel >= TotalLevels)
        {
            Debug.Log("In NextLevel(): No more levels to load, calling WinGame()");
            WinGame();
            return;
        }
        
        StartCoroutine(LoadNextLevel());
    }
    
    IEnumerator LoadNextLevel()
    {
        _scoreboard.SetActive(true);
        yield return new WaitForSeconds(2);
        hitCounterText.text = "0";
        
        SceneManager.LoadScene((CurrentLevel + 1).ToString());
        Debug.Log("Loading Level " + (CurrentLevel + 1));
        CurrentLevel++;
        _scoreboard.SetActive(false);
    }

    public void WinGame()
    {
        if (!GameWon)
        {
            GameWon = true;
            SceneManager.LoadScene("Win");
        }
    }

    public void IncreaseHitCounter()
    {
        if (SceneManager.GetActiveScene().name == "Practice")
        {
            return;
        }
        if (!GameWon)
        {
            HitCounter["Level " + CurrentLevel] += 1;
            hitCounterText.text = HitCounter["Level " + CurrentLevel].ToString();
        }
    }

    private void RefreshScoreboard()
    {
        if (SceneManager.GetActiveScene().name == "Practice") return;
        
        Debug.Log("Refreshing Scoreboard, Hitcounter looks like this: ");
        for (int i = 0; i < scoreboardTexts.Length ; i++)
        {
            scoreboardTexts[i].text = "Hole " + (i+1) + "\n \n" + HitCounter["Level " + (i + 1)];
            //Debug.Log("Score of Level " + (i + 1) + ": " + HitCounter["Level " + (i + 1)]);
        }
        totalScoreText.text = "Total\n \n" + CalculateTotalScore();
    }
    
    private int CalculateTotalScore()
    {
        int totalScore = 0;
        for (int i = 0; i < scoreboardTexts.Length; i++)
        {
            totalScore += HitCounter["Level " + (i + 1)];
        }
        return totalScore;
    }
    public void CheckMaxHits()
    {
        if (HitCounter["Level " + CurrentLevel] >= maxHitsPerLevel)
        {
            NextLevel();
        }
    }
}
