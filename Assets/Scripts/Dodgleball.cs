using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgleball : MonoBehaviour 
{
	private bool _isHeld = false;

	public bool isHeld
	{
		get { return _isHeld; }
		set { _isHeld = value; }
	}
}
