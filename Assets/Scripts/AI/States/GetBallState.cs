using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBallState : BaseState 
{
	private AIAgent _owner = null;
	private float _elapsedTime = 0.0f;

    private Dodgleball _lastBall;

	public GetBallState(AIAgent owner)
	{
        _stateDefinition.stateName = StateDefinition.StateName.GetBall;
		_owner = owner;
	}

	public override void OnEnter()
	{
		_elapsedTime = 0.0f;
        _lastBall = null;
	}

	public override void OnExit()
	{
        _lastBall = null;
	}

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;

        Transform closestBall = GetClosestBall();
        if (closestBall == null)
            _owner.SwitchState(StateDefinition.StateName.Hide);

        _owner.targetBall = closestBall;

        if (_owner.targetBall != null)
        {
            Vector3 direction = _owner.GetDirectionToTarget(_owner.transform.position, _owner.targetBall.position);

            if (!_owner.hasBall)
            {
                _owner.MoveForward(direction.normalized);
                CanGrabBall();
            }
        }
    }

    private Transform GetClosestBall()
    {
        Transform closestBall = null;
        float closestDistance = Mathf.Infinity;

        foreach (Dodgleball ball in _owner.dodgeballs)
        {
            if (ball.side == _owner.team && ball.active == false)
            {
                if (!ball.isHeld)
                {
                    float distance = Vector3.Distance(ball.transform.position, _owner.transform.position);

                    if (distance < closestDistance)
                    {
                        closestBall = ball.transform;
                        closestDistance = distance;
                    }
                }
            }
        }

        return closestBall;
    }

    private void CanGrabBall()
    {
        if(Vector3.Distance(_owner.transform.position, _owner.targetBall.position) < 1.1f)
        {
            _owner.PickUpBall();
        }
    }
}
