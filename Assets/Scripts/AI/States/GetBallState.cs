using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBallState : BaseState 
{
	private AIAgent _owner = null;

	private float _elapsedTime = 0.0f;

	public GetBallState(AIAgent owner)
	{
        _stateDefinition.stateName = StateDefinition.StateName.GetBall;
		_owner = owner;
	}

	public override void OnEnter()
	{
		Debug.Log("Entering SearchState");
		_elapsedTime = 0.0f;
	}

	public override void OnExit()
	{
		Debug.Log("Exiting SearchState");
	}

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;

        Vector3 direction = _owner.GetDirectionToTarget(_owner.transform.position, _owner.targetBall.position);

        if(!_owner.hasBall)
        {
            _owner.MoveForward(direction.normalized);
        }
    }
}
