using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
	private List<BaseState> _states = new List<BaseState>();

	private BaseState _currentState;
	private StateDefinition.StateName _desiredState;

	public BaseState currentState
	{
		get { return _currentState; }
		set { _currentState = value; }
	}

	public StateDefinition.StateName desiredState
	{
		get { return _desiredState; }
		set { _desiredState = value; }
	}

	public StateManager()
	{
		_desiredState = StateDefinition.StateName.Null;
	}

	public void Update()
	{
		StateDefinition.StateName currentTempState = _currentState != null ? _currentState.GetStateName() : StateDefinition.StateName.Null;

		if(_desiredState != currentTempState && _desiredState != StateDefinition.StateName.Null)
		{
			BaseState newState = GetState(_desiredState);

			if(newState != null)
			{
				if(_currentState != null)
				{
					_currentState.OnExit();
				}

				newState.OnEnter();
				_currentState = newState;
			}
		}

		if(_currentState != null)
		{
			_currentState.Update();
		}
	}

	public void AddState(BaseState state)
	{
		_states.Add(state);
	}

	private BaseState GetState(StateDefinition.StateName stateName)
	{
		foreach(BaseState state in _states)
		{
			if(state.GetStateName() == stateName)
			{
				return state;
			}
		}

		return null;
	}
}
