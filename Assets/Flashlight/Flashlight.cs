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
    private DifficultyManager difficultyManager;
    
    void Start()
    {
        difficultyManager = DifficultyManager.Instance;
        switch(difficultyManager.currentDifficulty)
        {
            case Difficulty.Amateur:
                _decayFactor = 2f;
                _enemyDetectionRange = 8f;
                _enemyKillTime = 0.6f;
                break;

            case Difficulty.Intermediate:
                _decayFactor = 2.5f;
                _enemyDetectionRange = 7f;
                _enemyKillTime = 0.7f;
                break;

            case Difficulty.Professional:
                _decayFactor = 3f;
                _enemyDetectionRange = 6f;
                _enemyKillTime = 0.8f;
                break;

            case Difficulty.Nightmare:
                _decayFactor = 3.5f;
                _enemyDetectionRange = 5f;
                _enemyKillTime = 1f;
                break;

            case Difficulty.Insanity:
                _decayFactor = 4f;
                _enemyDetectionRange = 5f;
                _enemyKillTime = 1.2f;
                break;

            default:
                Debug.LogWarning("Unknown difficulty");
                break;
        }
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
            timer = 0f;
            FlashlightManager.Instance._enemyDeathSlider.value = 0f;
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
