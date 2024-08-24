using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private GameManager _gameManager;
// Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            _gameManager.NextLevel();
    }
}
