using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour 
{
    private Rigidbody _rigidbody;

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
