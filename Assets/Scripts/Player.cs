using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    private Rigidbody _rigidbody;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        _rigidbody.MovePosition(newPosition);
    }
}
