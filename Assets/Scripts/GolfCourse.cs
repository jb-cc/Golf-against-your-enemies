using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfCourse : MonoBehaviour
{
    public Transform StartPosition { private set; get; }
    
    void Awake()
    {
        StartPosition = transform.Find("StartPosition");
    }
}

