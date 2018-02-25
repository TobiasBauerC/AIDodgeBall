using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour {

	private StateManager _stateManager = null;

	// Use this for initialization
	void Start () 
	{
		_stateManager = new StateManager();
		_stateManager.AddState(new IdleState());
		_stateManager.desiredState = StateDefinition.StateName.IdleState;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_stateManager.Update();
	}
}
