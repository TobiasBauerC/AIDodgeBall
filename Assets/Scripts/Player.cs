using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
    [SerializeField] private Transform _ballParent;
    [SerializeField] private Transform _targetArrow;
    [SerializeField] private Text _scoreText;
    [SerializeField] private UIBallMover _ballMover;
    [SerializeField] private List<Target> _targets = new List<Target>();
    [SerializeField] private List<Baseball> _balls = new List<Baseball>();

    private Baseball _currentBall;
    private Target _currentTarget;
    private int _currentBallIndex = 0;
    private int _currentTargetIndex = 0;
    private int _score = 0;

    private bool _canThrow = true;
    private bool _hasBalls = true;

	// Use this for initialization
	void Start () 
    {
        _currentBall = _balls[_currentBallIndex];
        _currentTarget = _targets[_currentTargetIndex];
        _currentBall.PickUp(_ballParent);
        PositionTargetArrow();
        UpdateScoreText();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextTarget(1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            NextTarget(-1);

        if(!_hasBalls || _targets.Count <= 0)
        {
            PhysicsGameManager.instance.GameOver();
            return;
        }

        if (!_canThrow)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space))
            ThrowBall();     
    }

	private void LateUpdate()
	{
        PositionTargetArrow();
	}

    public void ChangeScore(int amount)
    {
        _score += amount;
        UpdateScoreText();
    }

    public void TargetHit(Target hitTarget)
    {
        if(hitTarget == _currentTarget)
            NextTarget(1);
        _targets.Remove(hitTarget);
        Destroy(hitTarget.gameObject);
    }

	private void NextTarget(int direction)
    {
        if (_targets.Count <= 0)
            return;

        _currentTargetIndex += direction;

        if (_currentTargetIndex > _targets.Count - 1)
            _currentTargetIndex = 0;
        else if (_currentTargetIndex < 0)
            _currentTargetIndex = _targets.Count - 1;

        _currentTarget = _targets[_currentTargetIndex];
        PositionTargetArrow();
    }

    private void NextBall()
    {
        _currentBallIndex++;
        if (_currentBallIndex > _balls.Count - 1)
        {
            _hasBalls = false;
            return;
        }
        _currentBall = _balls[_currentBallIndex];
        _currentBall.PickUp(_ballParent);
    }

    private void PositionTargetArrow()
    {
        if (_targets.Count <= 0)
            return;
        Vector3 newPos = _currentTarget.transform.position;
        newPos.y += 1.2f;
        _targetArrow.position = newPos;
    }

    private void ThrowBall()
    {
        _ballMover.StopBall();
        _currentBall.Throw(22.22f, 1.0f, _ballMover.GetVerticalMod(), _currentTarget, this);
        StartCoroutine("ThrowWait");
    }

    private void UpdateScoreText()
    {
        _scoreText.text = string.Format("SCORE: {0}", _score);
    }

    private IEnumerator ThrowWait()
    {
        _canThrow = false;
        yield return new WaitForSeconds(2.0f);
        if (_hasBalls)
        {
            NextBall();
            _ballMover.StartBall();
            _canThrow = true;
        }
    }
}
