using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMover : MonoBehaviour 
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

	// Use this for initialization
	void Start () 
    {
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
        _rigidbody.velocity = Vector3.up * _speed;

        if ((transform.position.y <= _minY && _speed <= -0.1f) || (transform.position.y >= _maxY && _speed >= 0.1f))
            _speed *= -1.0f;
            
	}
}
