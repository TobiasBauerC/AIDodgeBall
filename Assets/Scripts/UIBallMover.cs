using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallMover : MonoBehaviour 
{
    [SerializeField] private Slider _ballSlider;
    [SerializeField] private float _speed;

    private bool _moving = true;
	
	// Update is called once per frame
	void Update () 
    {
        if (!_moving)
            return;

        _ballSlider.value += _speed * Time.deltaTime;

        if (_ballSlider.value <= _ballSlider.minValue || _ballSlider.value >= _ballSlider.maxValue)
            _speed *= -1.0f;
	}

    public void StopBall()
    {
        _moving = false;
    }

    public void StartBall()
    {
        _ballSlider.value = _ballSlider.minValue;
        _moving = true;
    }

    public float GetVerticalMod()
    {
        if (_ballSlider.value < 45.0f)
            return -5.0f;
        if (_ballSlider.value > 55.0f)
            return 5.0f;
        return 0.0f;
    }
}
