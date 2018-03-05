using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class AIAgent : MonoBehaviour 
{
    [SerializeField] private Team _team;
	[SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private Transform _handLocation = null;
	[Header("Movement")]
	[SerializeField] private float _linearSpeed = 5.0f;
	[SerializeField] private float _angularSpeed = 5.0f;
	[SerializeField] private List<Dodgleball> _dodgeballs = new List<Dodgleball>();
    [SerializeField] private List<AIAgent> _enemyAgents = new List<AIAgent>();

	private StateManager _stateManager = null;
	private Transform _targetBall = null;
    private Transform _targetAgent = null;

    public bool hasBall { get; set; }

	public Transform targetBall
	{
		get { return _targetBall; }
		set { _targetBall = value; }
	}

    public Transform targetAgent
    {
        get { return _targetAgent; }
        set { _targetAgent = value; }
    }

	public List<Dodgleball> dodgeballs
	{
		get
		{
			if(_dodgeballs.Count <= 0 || _dodgeballs.Contains(null))
			{
				_dodgeballs.Clear();

				GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

				foreach(GameObject d in balls)
				{
					_dodgeballs.Add(d.GetComponent<Dodgleball>());
				}
			}

			return _dodgeballs;
		}
	}

    public List<AIAgent> enemyAgents
    {
        get
        {
            if (_enemyAgents.Count == 0 || _enemyAgents.Contains(null))
            {
                _enemyAgents.Clear();

                if (_team == Team.blue)
                {
                    foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Red"))
                    {
                        _enemyAgents.Add(agent.GetComponent<AIAgent>());
                    }
                }
            }

            return _enemyAgents;
        }
    }

    private enum Team
    {
        blue,
        red
    }

	void Start () 
	{
		if(!_rigidbody)
			_rigidbody = GetComponent<Rigidbody>();
        if (!_handLocation)
            Debug.LogError("No Hand Location in AIAgent inspector!!");

        hasBall = false;

		_stateManager = new StateManager();
		_stateManager.AddState(new SearchState(this));
		_stateManager.AddState(new GetBallState(this));
		_stateManager.desiredState = StateDefinition.StateName.Search;
	}

	void Update () 
	{
		_stateManager.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _stateManager.desiredState = StateDefinition.StateName.GetBall;
        }
	}

    /// <summary>
    /// Moves the agent in the given direction.
    /// </summary>
	public void MoveForward(Vector3 direction)
	{
		if(direction.magnitude != 1.0f)	
			direction.Normalize();

		_rigidbody.velocity = direction * _linearSpeed;
	}

    /// <summary>
    /// Rotates the agent clockwise
    /// </summary>
	public void TurnRight()
	{
		_rigidbody.angularVelocity = transform.up * _angularSpeed;
	}

    /// <summary>
    /// Rotates the agent counter clockwise
    /// </summary>
	public void TurnLeft()
	{
		_rigidbody.angularVelocity = transform.up * _angularSpeed * -1.0f;
	}

    /// <summary>
    /// Stops the linear velocity.
    /// </summary>
	public void StopLinearVelocity()
	{
		Vector3 stopLinearVelocity = _rigidbody.velocity;
		stopLinearVelocity.x = 0.0f;
		stopLinearVelocity.z = 0.0f;
		_rigidbody.velocity = stopLinearVelocity;
	}

    /// <summary>
    /// Stops the angular velocity.
    /// </summary>
	public void StopAngularVelocity()
	{
		_rigidbody.angularVelocity = Vector3.zero;
	}

    /// <summary>
    /// Stops all movement.
    /// </summary>
    public void StopAllMovement()
    {
        StopLinearVelocity();
        StopAngularVelocity();
    }

    /// <summary>
    /// Gets the direction to target.
    /// </summary>
    /// <returns>The direction to target.</returns>
    /// <param name="start">Start.</param>
    /// <param name="target">Target.</param>
    public Vector3 GetDirectionToTarget(Vector3 start, Vector3 target)
    {
        return target - start;
    }

	private void OnCollisionEnter(Collision c)
	{
        if(c.gameObject.tag == "Ball")
        {
            hasBall = true;
            c.gameObject.GetComponent<Dodgleball>().PickUp(_handLocation);
        }
	}
}
