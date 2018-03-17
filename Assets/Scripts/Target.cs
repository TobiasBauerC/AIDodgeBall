using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour 
{
    [SerializeField] private int _scoreValue;
    private Rigidbody _rigidbody;

    public int scoreValue { get { return _scoreValue; } }

	// Use this for initialization
	void Start () 
    {
        _rigidbody = GetComponent<Rigidbody>();	
	}

	public Vector3 GetVelocity()
    {
        return _rigidbody.velocity;
    }
}
