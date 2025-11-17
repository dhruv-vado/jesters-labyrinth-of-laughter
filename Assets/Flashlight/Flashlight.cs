using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]private Light _flashlight;
    [HideInInspector]public bool _isOn;
    [SerializeField]private float _totalLife;
    [SerializeField]private float _decayFactor;
    [SerializeField]private LayerMask _enemyDetectionMask;
    [SerializeField]private float _enemyDetectionRange; 
    [SerializeField]private float _enemyKillTime = 1f;
    private float timer = 0f;

    private Enemy _enemy;
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
            if(RayCheck())
            {
                timer += Time.deltaTime;
                FlashlightManager.Instance._enemyDeathSlider.value = timer / _enemyKillTime;
                if(timer >= _enemyKillTime)
                {
                    _enemy.EnemyDied();
                    timer = 0f;
                    FlashlightManager.Instance._enemyDeathSlider.value = 0f;
                }
            }
            else
            {
                timer = 0f;
                FlashlightManager.Instance._enemyDeathSlider.value = 0f;
            }

            if(_lifeLeft <= 0)
            {
                _flashlight.gameObject.SetActive(false);
                FlashlightManager.Instance._enemyDeathSlider.value = 0f;
            }
        }
        else
        {
            _flashlight.enabled = false;
        }
    }

    private bool RayCheck()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, _enemyDetectionRange, _enemyDetectionMask, QueryTriggerInteraction.Ignore) && hitInfo.collider.CompareTag("Enemy"))
        {
            _enemy = hitInfo.collider.GetComponent<Enemy>();
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * _enemyDetectionRange);
    }
}
