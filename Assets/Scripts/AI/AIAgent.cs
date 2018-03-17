using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]

public class AIAgent : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private Transform _handLocation = null;
    [SerializeField] private LayerMask _agentMask;
    [Header("Movement")]
    [SerializeField] private float _linearSpeed = 5.0f;
    [SerializeField] private float _angularSpeed = 5.0f;
    [SerializeField] private List<Dodgleball> _dodgeballs = new List<Dodgleball>();
    [SerializeField] private List<AIAgent> _enemyAgents = new List<AIAgent>();
    [Header("Location")]
    [SerializeField] private Transform _blueSide;
    [SerializeField] private Transform _redSide;

    private StateManager _stateManager = null;
    private Transform _targetBall = null;
    private Transform _targetAgent = null;
    private bool _running = false;

    public bool hasBall { get; set; }

    public Team team
    {
        get { return _team; } 
    }

    public LayerMask agentMask
    {
        get { return _agentMask; }
    }

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
                else if (_team == Team.red)
                {
                    foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Blue"))
                    {
                        _enemyAgents.Add(agent.GetComponent<AIAgent>());
                    }
                }
            }

            return _enemyAgents;
        }
    }

    public enum Team
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
		_stateManager.AddState(new GetBallState(this));
        _stateManager.AddState(new ThrowBallState(this));
        _stateManager.AddState(new HideState(this));
        _stateManager.desiredState = StateDefinition.StateName.GetBall;
	}

	void Update () 
	{
        if (!_running)
        {
            StopAllMovement();
            return;
        }

		_stateManager.Update();
	}

    public void ActivateAgent()
    {
        _running = !_running;
    }

    /// <summary>
    /// Switchs the state.
    /// </summary>
    public void SwitchState(StateDefinition.StateName state)
    {
        _stateManager.desiredState = state;
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

    public Vector3 GetVelocity()
    {
        Vector3 slightlyOffVelocity = _rigidbody.velocity;
        float slightAdjustment = Random.Range(-2.0f, 2.0f);

        slightlyOffVelocity *= slightAdjustment;

        return slightlyOffVelocity;
    }

    /// <summary>
    /// Gets the direction to target.
    /// </summary>
    public Vector3 GetDirectionToTarget(Vector3 start, Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        bool isSuccess = NavMesh.CalculatePath(start, target, NavMesh.AllAreas, path);
        if (isSuccess)
        {
            return target - start;
        }

        _stateManager.desiredState = StateDefinition.StateName.Hide;

        return Vector3.zero;
    }

    public void ThrowBall()
    {
        targetBall.GetComponent<Dodgleball>().Throw(22.22f, 1.0f, targetAgent.GetComponent<AIAgent>(), this);
        _targetBall = null;
        _targetAgent = null;
        hasBall = false;
    }

    public void PickUpBall()
    {
        StopAllMovement();
        SwitchState(StateDefinition.StateName.ThrowBall);
        targetBall.GetComponent<Dodgleball>().PickUp(_handLocation);
        targetBall.GetComponent<Dodgleball>().isHeld = true;
        hasBall = true;
    }

    public bool OnMySide(Vector3 position)
    {
        if(Vector3.Distance(position, _blueSide.position) < Vector3.Distance(position, _redSide.position))
        {
            if (team == Team.blue)
                return true;
           
            return false;
        }

        if (team == Team.blue)
            return false;
                
        return true;
    }

	private void OnCollisionEnter(Collision c)
	{
        if(c.gameObject.tag == "Ball")
        {
            Dodgleball ball = c.gameObject.GetComponent<Dodgleball>();

            if (ball.active == true && ball.thrower.team != _team)
            {
                int catchBall = (int) Random.Range(1.0f, 5.0f);

                if(catchBall == 1)
                {
                    _targetBall = ball.transform;
                    PickUpBall();

                    switch(_team)
                    {
                        case Team.blue:
                            GameManager.instance.AddScore(Team.blue, 1);
                            break;

                        case Team.red:
                            GameManager.instance.AddScore(Team.red, 1);
                            break;
                    }
                }
                else
                    Destroy(gameObject);
            }
        }
	}
}
