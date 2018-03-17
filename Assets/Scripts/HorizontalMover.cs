using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMover : MonoBehaviour 
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    // Use this for initialization
    void Start()
    {
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.right * _speed;

        if ((transform.position.x <= _minX && _speed <= -0.1f) || (transform.position.x >= _maxX && _speed >= 0.1f))
        {
            _speed *= -1.0f;
        }
    }
}
