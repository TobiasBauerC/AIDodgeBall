using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetState : BaseState
{
    private AIAgent _owner = null;

    private float _elapsedTime = 0.0f;

    public FindTargetState(AIAgent owner)
    {
        _stateDefinition.stateName = StateDefinition.StateName.FindTarget;
        _owner = owner;
    }

    public override void OnEnter()
    {
        Debug.Log("Entering FindTargetState");
        _elapsedTime = 0.0f;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting FindTargetState");
    }

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;
    }
}
