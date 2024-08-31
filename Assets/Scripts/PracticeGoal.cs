using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeGoal : MonoBehaviour
{
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            _gameManager.StartPractice();
    }
}
