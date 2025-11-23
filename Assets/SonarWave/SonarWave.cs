using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarWave : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private float _initialRadius = 0.5f;
    [SerializeField] private float _maxRadius = 30f;
    [SerializeField] private float _expansionSpeed = 15f;
    [SerializeField] public float _maxIntensity = 2f;
    [SerializeField] private float _maxIntensityAtChase = 1f;
    [SerializeField] private float _intensityFadeStartPercent = 0.7f;
    [SerializeField] private float _cooldown = 2.5f;
    public Light _light;

    float _currentMaxIntensity;

    private InputManager _inputManager;

    public float _currentRadius;
    public bool _isActive = false;
    public bool _isFadingOut = false;
    private float _lastSonarTime = -999f;

    private void Start()
    {
        _currentRadius = _initialRadius;
        _light.range = _initialRadius;
        _light.intensity = 0f;
        _inputManager = InputManager.Instance;
    }
    
    private void Update()
    {
        float timeSinceLastSonar = Time.time - _lastSonarTime;
        
        if(_inputManager.Sonar() && timeSinceLastSonar >= _cooldown && !_isActive)
        {
            StartSonarPulse();
        }
        
        if(EnemyManager.Instance._playerDetected)
        {
            _currentMaxIntensity = _maxIntensityAtChase;
        }
        else
        {
            _currentMaxIntensity = _maxIntensity;
        }

        if(_isActive)
        {
            if(!_isFadingOut)
            {
                _currentRadius += _expansionSpeed * Time.deltaTime;
                _light.range = _currentRadius;
                
                float expansionPercent = _currentRadius / _maxRadius;
                
                if(expansionPercent >= _intensityFadeStartPercent)
                {
                    float fadeProgress = (expansionPercent - _intensityFadeStartPercent) / (1f - _intensityFadeStartPercent);
                    _light.intensity = _currentMaxIntensity * (1f - fadeProgress);
                }
                else
                {
                    _light.intensity = _currentMaxIntensity;
                }
                
                if(_currentRadius >= _maxRadius)
                {
                    _isFadingOut = true;
                    ResetSonar();
                }
            }
        }
    }
    
    private void StartSonarPulse()
    {
        _isActive = true;
        _isFadingOut = false;
        _currentRadius = _initialRadius;
        _light.range = _initialRadius;
        _light.intensity = _currentMaxIntensity;
        _lastSonarTime = Time.time;        
    }
    
    private void ResetSonar()
    {
        _isActive = false;
        _isFadingOut = false;
        _currentRadius = _initialRadius;
        _light.range = _initialRadius;
        _light.intensity = 0f;
    }   
}

