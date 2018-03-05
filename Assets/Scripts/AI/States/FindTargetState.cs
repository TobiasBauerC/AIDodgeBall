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

        Transform closestEnemy = GetClosestEnemy();
        _owner.targetAgent = closestEnemy;
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
