using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBallState : BaseState 
{
    private AIAgent _owner;

    private float _elapsedTime = 0.0f;

    public ThrowBallState(AIAgent owner)
    {
        _stateDefinition.stateName = StateDefinition.StateName.ThrowBall;
        _owner = owner;
    }

	public override void OnEnter()
	{
        _elapsedTime = 0.0f;
	}

	public override void OnExit()
	{
        Debug.Log("Exiting ThrowBallState");
	}

	public override void Update()
	{
        _elapsedTime += Time.deltaTime;

        Transform closestEnemy = GetClosestEnemy();
        _owner.targetAgent = closestEnemy;

        if(_owner.targetBall && _owner.targetAgent)
        {
            _owner.ThrowBall();
        }

        if(_elapsedTime <= 1.0f)
        {
            return;
        }

        if(_owner.targetBall == null)
        {
            _owner.SwitchState(StateDefinition.StateName.GetBall);
        }
	}

    private Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (AIAgent enemy in _owner.enemyAgents)
        {
            float distance = Vector3.Distance(enemy.transform.position, _owner.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = enemy.transform;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }
}
