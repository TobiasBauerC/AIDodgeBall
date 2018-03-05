using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgleball : MonoBehaviour 
{
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isHeld = false;

	public bool isHeld
	{
		get { return _isHeld; }
		set { _isHeld = value; }
	}

	private void Start()
	{
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody>();
	}

    /// <summary>
    /// Picks up this ball.
    /// </summary>
    /// <param name="parent">Parent.</param>
	public void PickUp(Transform parent)
    {
        _rigidbody.isKinematic = true;
        transform.position = parent.position;
        transform.parent = parent;
    }
}
