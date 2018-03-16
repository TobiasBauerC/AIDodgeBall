using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgleball : MonoBehaviour 
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _blueSide;
    [SerializeField] private Transform _redSide;

    private bool _isHeld = false;

    // Throwing ball vars:
    private float _minSpeed;
    private AIAgent _movingTarget = null;
    private AIAgent _thrower = null;
    private float _desiredAirTime = 1.0f;
    //

    public AIAgent thrower
    {
        get { return _thrower; }
    }

	public bool isHeld
	{
		get { return _isHeld; }
		set { _isHeld = value; }
	}

    public bool active
    {
        get;
        private set;
    }

    public AIAgent.Team side
    {
        get
        {
            if(Vector3.Distance(transform.position, _blueSide.position) < Vector3.Distance(transform.position, _redSide.position))
            {
                return AIAgent.Team.blue;
            }
            else
            {
                return AIAgent.Team.red;
            }
        }
    }

	private void Start()
	{
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody>();
        if (!_collider)
            _collider = GetComponent<Collider>();

        Vector3 newPos = Vector3.zero;
        newPos.x = Random.Range(-16.0f, 16.0f);
        newPos.z = Random.Range(-24.0f, 24.0f);
        newPos.y = 1.0f;

        transform.position = newPos;

        active = false;
	}

	private void LateUpdate()
	{
        if(transform.position.y <= -1.0f)
        {
            Vector3 pos = transform.position;
            pos.y = 1.0f;

            transform.position = pos;
        }
	}

	/// <summary>
	/// Picks up this ball.
	/// </summary>
	public void PickUp(Transform parent)
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        transform.position = parent.position;
        transform.parent = parent;
    }

    public void Throw(float minSpeed, float desiredAirTime, AIAgent movingTarget, AIAgent thrower)
    {
        _collider.enabled = true;
        active = true;
        _minSpeed = minSpeed;
        _desiredAirTime = desiredAirTime;
        _movingTarget = movingTarget;
        _thrower = thrower;

        _rigidbody.isKinematic = false;
        transform.parent = null;
        isHeld = false;

        _rigidbody.velocity = CalculateInitialVelocityMovingTarget();
    }



    /////////////////////////////////////////////////////////////////////////////



    Vector3 CalculateInitialVelocityMovingTarget()
    {
        //find out where the target will be in our desired time
        //aim for that position
        Vector3 targetVelocity = _movingTarget.GetVelocity();
        Vector3 targetDisplacement = targetVelocity * _desiredAirTime;
        Vector3 targetPosition = _movingTarget.transform.position + targetDisplacement;
        return CalculateInitialVelocity(targetPosition, true);
    }

    Vector3 CalculateInitialVelocity(Vector3 targetPosition, bool useDesiredTime)
    {
        Vector3 displacement = targetPosition - this.transform.position;
        float yDisplacement = displacement.y;
        displacement.y = 0.0f;
        float horizontalDisplacement = displacement.magnitude;
        if (horizontalDisplacement < Mathf.Epsilon)
        {
            return Vector3.zero;
        }

        //v = d/t
        //vt = d
        //t = d/v

        float horizontalSpeed = useDesiredTime ? horizontalDisplacement / _desiredAirTime : _minSpeed;

        float time = horizontalDisplacement / horizontalSpeed;
        //we know the time it requires to reach the target
        //we need the initial velocity, that can ensure the
        //projectile gets airborn for half that time
        //1/2 ascending 1/2 descend 
        time *= 0.5f;
        //a = v/t
        //at = v
        //v is delta velocity, Vf - Vi
        //final velocity is 0, it is the peak of our upward travel

        //-Vi = at
        //Vi = -at
        Vector3 initialYVelocity = Physics.gravity * time * -1.0f;
        //assuming min velocity is a flat vector
        displacement.Normalize();
        Vector3 initialHorizontalVelocity = displacement * horizontalSpeed;
        return initialHorizontalVelocity + initialYVelocity;
    }

	private void OnCollisionEnter(Collision collision)
	{
        StartCoroutine("Deactive");
	}

    IEnumerator Deactive()
    {
        yield return new WaitForEndOfFrame();
        active = false;
    }
}
