using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour 
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    private Player _player;

    // Throwing ball vars:
    private float _minSpeed;
    private Target _movingTarget = null;
    private float _desiredAirTime = 1.0f;
    private float _verticalMod = 0.0f;
    //

    private void Start()
    {
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody>();
        if (!_collider)
            _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        
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

    /// <summary>
    /// Throw the ball.
    /// </summary>
    public void Throw(float minSpeed, float desiredAirTime, float verticalMod, Target movingTarget, Player player)
    {
        _collider.enabled = true;
        _minSpeed = minSpeed;
        _desiredAirTime = desiredAirTime;
        _verticalMod = verticalMod;
        _movingTarget = movingTarget;
        _player = player;

        _rigidbody.isKinematic = false;
        transform.parent = null;

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
        displacement.y += _verticalMod;
        //displacement.y = 0.0f;
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
        //we know the time it requires to reach the target.
        //we need the initial velocity, that can ensure the...
        //...projectile gets airborn for half that time.
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

	private void OnCollisionEnter(Collision c)
	{
        if(c.gameObject.tag == "Target")
        {
            Target target = c.gameObject.GetComponent<Target>();
            if(target == _movingTarget)
            {
                _player.ChangeScore(target.scoreValue);
                _player.TargetHit(target);
            }
            else
            {
                _player.ChangeScore(-target.scoreValue);
            }
        }
	}
}
