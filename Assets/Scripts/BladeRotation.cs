using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 69f;
    
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
    }
}
