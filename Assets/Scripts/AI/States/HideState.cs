using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : BaseState 
{
    private AIAgent _owner = null;
    private Vector3 _targetLocation;
    private float _elapseTime = 0.0f;
    private float _hideTime = 0.0f;

    public HideState(AIAgent owner)
    {
        _owner = owner;
        _stateDefinition.stateName = StateDefinition.StateName.Hide;
    }

	public override void OnEnter()
	{
        _elapseTime = 0.0f;
        _hideTime = Random.Range(0.5f, 5.0f);
        _targetLocation = GetNewLocation();
	}

	public override void OnExit()
	{
	}

	public override void Update()
	{
        _elapseTime += Time.deltaTime;

        if (_elapseTime >= _hideTime)
        {
            _owner.SwitchState(StateDefinition.StateName.GetBall);
        }

        if(CloseBall())
            _owner.SwitchState(StateDefinition.StateName.GetBall);

        // if at position, go to a different one //
        if (Vector3.Distance(_targetLocation, _owner.transform.position) < 0.3f)
            _targetLocation = GetNewLocation();

        // move to hide position //
        Vector3 direction = _owner.GetDirectionToTarget(_owner.transform.position, _targetLocation);
        _owner.MoveForward(direction.normalized);
	}

    public Vector3 GetNewLocation()
    {
        Vector3 newLocation = Vector3.zero;

        newLocation.x = Random.Range(-14.0f, 14.0f);
        newLocation.z = Random.Range(-27.0f, 27.0f);
        newLocation.y = _owner.transform.position.y;

        if (!_owner.OnMySide(newLocation))
            return GetNewLocation();

        return newLocation;
    }

    private bool CloseBall()
    {
        Transform closestBall = null;
        float closestDistance = 5.0f;

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

        return closestBall == null ? false : true;
    }
}
