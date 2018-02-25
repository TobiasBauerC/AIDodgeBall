using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState 
{
	private float _elapsedTime = 0.0f;

	public IdleState()
	{
		_stateDefinition.stateName = StateDefinition.StateName.IdleState;
	}

	public override void OnEnter()
	{
		Debug.Log("Entering IdleState");
		_elapsedTime = 0.0f;
	}

	public override void OnExit()
	{
		Debug.Log("Exiting IdleState");
	}

	public override void Update()
	{
		Debug.Log("Updating IdleState");
		_elapsedTime += Time.deltaTime;
	}
}
