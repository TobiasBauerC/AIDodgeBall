using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState 
{
	private AIAgent _owner = null;
	private Transform _target = null;

	private float _elapsedTime = 0.0f;

	public SearchState(AIAgent owner)
	{
		_stateDefinition.stateName = StateDefinition.StateName.Search;
		_owner = owner;
	}

	public override void OnEnter()
	{
		Debug.Log("Entering SearchState");
		_target = null;
		_elapsedTime = 0.0f;
	}

	public override void OnExit()
	{
		Debug.Log("Exiting SearchState");
	}

	public override void Update()
	{
		_elapsedTime += Time.deltaTime;

		Transform closestBall = GetClosestBall();
		_owner.targetBall = closestBall;
	}

    /// <summary>
    /// Gets the closest ball.
    /// </summary>
    /// <returns>The closest ball.</returns>
	private Transform GetClosestBall()
	{
		Transform closestBall = null;
		float closestDistance = Mathf.Infinity;

		foreach(Dodgleball ball in _owner.dodgeballs)
		{
			if(!ball.isHeld)
			{
				float distance = Vector3.Distance(ball.transform.position, _owner.transform.position);

				if(distance < closestDistance)
				{
					closestBall = ball.transform;
					closestDistance = distance;
				}
			}
		}

		return closestBall;
	}
}
