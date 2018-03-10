using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : BaseState 
{
    private AIAgent _owner = null;
    private float _elapseTime = 0.0f;


	public override void OnEnter()
	{
        Debug.Log("Entering HideState");
        _elapseTime = 0.0f;
	}

	public override void OnExit()
	{
        Debug.Log("Exiting HideState");
	}

	public override void Update()
	{
        _elapseTime += Time.deltaTime;
	}
}
