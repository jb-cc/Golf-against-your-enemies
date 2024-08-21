using UnityEngine;

public class GolfCourse : MonoBehaviour
{
    public Transform StartPosition { private set; get; }

    
    void Awake()
    {
        StartPosition = transform.Find("Starting Mat").transform;
        StartPosition = StartPosition.transform.Find("StartPosition").transform;
        Debug.Log("Start position: " + StartPosition.position);
    }
}

