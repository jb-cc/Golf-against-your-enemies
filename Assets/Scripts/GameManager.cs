using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GolfCourse[] courses;
    public Player player;

    private void Start()
    {
        // Set the player's position to the first course's start position to start the game
        if (player != null && courses.Length > 0)
        {
            player.SetNewPosition(courses[0].StartPosition.position);
        }
    }
}
