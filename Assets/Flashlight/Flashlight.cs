using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]private Light _flashlight;
    [HideInInspector]public bool _isOn;
    [SerializeField]private float _totalLife;
    [SerializeField]private float _decayFactor;
    private float _lifeLeft;
    private float _batteryPercentage;
    private InputManager inputManager;
    
    void Start()
    {
        _flashlight.enabled = false;
        inputManager = InputManager.Instance;
        _lifeLeft = _totalLife;
        _batteryPercentage = 100f;
    }

    void Update()
    {
        if(inputManager.Flashlight())
        {
            _flashlight.enabled = true;
            _lifeLeft = _lifeLeft - _decayFactor * Time.deltaTime;
            _batteryPercentage = _lifeLeft * 100 / _totalLife;
            FlashlightManager.Instance.BatteryPercentage = Mathf.Round(_batteryPercentage);
        }
        else
        {
            _flashlight.enabled = false;
        }
    }
}
