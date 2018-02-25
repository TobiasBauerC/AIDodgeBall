using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
	protected StateDefinition _stateDefinition = new StateDefinition();

	public abstract void OnEnter();
	public abstract void OnExit();
	public abstract void Update();

	public StateDefinition.StateName GetStateName()
	{
		return _stateDefinition.stateName;
	}
}
