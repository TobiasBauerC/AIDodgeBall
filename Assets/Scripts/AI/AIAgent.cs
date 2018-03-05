using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class AIAgent : MonoBehaviour 
{
	[SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private Transform _handLocation = null;
	[Header("Movement")]
	[SerializeField] private float _linearSpeed = 5.0f;
	[SerializeField] private float _angularSpeed = 5.0f;
	[SerializeField] private List<Dodgleball> _dodgeballs = new List<Dodgleball>();

	private StateManager _stateManager = null;
	private Transform _targetBall = null;

    public bool hasBall { get; set; }

	public Transform targetBall
	{
		get { return _targetBall; }
		set { _targetBall = value; }
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

	public void MoveForward(Vector3 direction)
	{
		if(direction.magnitude != 1.0f)	
			direction.Normalize();

		_rigidbody.velocity = direction * _linearSpeed;
	}

	public void TurnRight()
	{
		_rigidbody.angularVelocity = transform.up * _angularSpeed;
	}

	public void TurnLeft()
	{
		_rigidbody.angularVelocity = transform.up * _angularSpeed * -1.0f;
	}

	public void StopLinearVelocity()
	{
		Vector3 stopLinearVelocity = _rigidbody.velocity;
		stopLinearVelocity.x = 0.0f;
		stopLinearVelocity.z = 0.0f;
		_rigidbody.velocity = stopLinearVelocity;
	}

	public void StopAngularVelocity()
	{
		_rigidbody.angularVelocity = Vector3.zero;
	}

    public void StopAllMovement()
    {
        StopLinearVelocity();
        StopAngularVelocity();
    }

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
