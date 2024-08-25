using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    private EventSystem _eventSystem;
    private GameObject _canvas;
    [SerializeField] private GameObject startMenu;
    private GameObject _scoreboard;
    [SerializeField] private TextMeshProUGUI[] scoreboardTexts;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI hitCounterText;
    public int CurrentLevel { get; private set; } = 0;
    public int TotalLevels { get; private set; } = 13;
    public bool GameWon { get; private set; } = false;
    public Dictionary<string, int> HitCounter { private set; get; } = new Dictionary<string, int>();
    public int maxHitsPerLevel = 12;
    void Awake()
    {
        if (_eventSystem == null)
        {
            _eventSystem = FindObjectOfType<EventSystem>();
        }
        if (_canvas == null)
        {
            _canvas = GameObject.Find("Canvas");
        }
        if (_scoreboard == null)
        {
            _scoreboard = _canvas.transform.Find("Scoreboard").gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_eventSystem.gameObject);
        DontDestroyOnLoad(_canvas.gameObject);
        
        
        // Initialize the hit counter for each level to 0
        for (int i=0; i<TotalLevels; i++)
        {
            HitCounter.Add("Level " + i, 0);
        }
        RefreshScoreboard();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "START")
        {
            Debug.Log("In Start(): GameManager Start, Start Menu Loaded");
            SceneManager.LoadScene("Menu");
            Debug.Log("In Start(): GameManager Awake, Menu Loaded");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            _scoreboard.SetActive(true); 
        }
        else
        {
            _scoreboard.SetActive(false);
        }
    }

    public void StartGame()
    {
        Debug.Log("In StartGame(): Starting Game");
        SceneManager.LoadScene("0");
    }
    
    public void QuitGame()
    {
        Application.Quit();
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
        hitCounterText.text = "0";
        
        SceneManager.LoadScene((CurrentLevel + 1).ToString());
        Debug.Log("Loading Level " + (CurrentLevel + 1));
        CurrentLevel++;
        
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
        if (!GameWon)
        {
            HitCounter["Level " + CurrentLevel] += 1;
            Debug.Log("Hit Counter: " + HitCounter["Level " + CurrentLevel]);
            Debug.Log("Current Level: " + CurrentLevel);
            Debug.Log("Hitcounter text: " + hitCounterText.text);
            hitCounterText.text = HitCounter["Level " + CurrentLevel].ToString();
        }
    }

    private void RefreshScoreboard()
    {
        Debug.Log("Refreshing Scoreboard, Hitcounter looks like this: ");
        for (int i = 0; i < scoreboardTexts.Length ; i++)
        {
            scoreboardTexts[i].text = "Hole " + (i+1) + "\n \n" + HitCounter["Level " + (i + 1)];
            Debug.Log("Score of Level " + (i + 1) + ": " + HitCounter["Level " + (i + 1)]);
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
