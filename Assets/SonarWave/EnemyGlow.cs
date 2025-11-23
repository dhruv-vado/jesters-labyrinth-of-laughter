using UnityEngine;

public class EnemyGlow : MonoBehaviour
{
    [Header("Enemy Glow")]
    [SerializeField] private float _emissionStrength = 5f;
    [SerializeField] private float _transitionSpeed = 5f;
    [SerializeField] private SphereCollider _detectionCollider;
    [SerializeField] private Material _enemyMaterial;
    [SerializeField] private SonarWave _sonarWave;

    private float _currentEmissionLerp = 0f;
    private bool _enemyDetected = false;
    private Collider _trackedEnemy = null;

    private void Start()
    {
        // Initialize collider settings
        if(_detectionCollider != null)
        {
            _detectionCollider.isTrigger = true;
            _detectionCollider.enabled = false;
            _detectionCollider.radius = 0.5f;
        }
        
        // Check for Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        // Initialize material
        if(_enemyMaterial != null)
        {
            _enemyMaterial.SetColor("_EmissionColor", Color.black);
        }
    }

    private void Update()
    {
        // Null checks to prevent errors
        if(_sonarWave == null || _enemyMaterial == null)
            return;
            
        if(_sonarWave._isActive)
        {
            if(!_sonarWave._isFadingOut)
            {                
                // Enable and update collider when sonar is active
                if(_detectionCollider != null)
                {
                        _detectionCollider.enabled = true;
                    _detectionCollider.radius = _sonarWave._currentRadius;
                }
                
                // Smooth transition for emission
                float targetLerp = _enemyDetected ? 1f : 0f;
                _currentEmissionLerp = Mathf.MoveTowards(_currentEmissionLerp, targetLerp, _transitionSpeed * Time.deltaTime);
                
                if(_currentEmissionLerp > 0f)
                {
                    _enemyMaterial.EnableKeyword("_EMISSION");

                    if(_sonarWave._light != null)
                    {
                        float intensityFactor = Mathf.Approximately(_sonarWave._maxIntensity, 0f) ? 0f : Mathf.Clamp01(_sonarWave._light.intensity / _sonarWave._maxIntensity);
                        float currentEmissionStrength = _emissionStrength * intensityFactor * _currentEmissionLerp;
                                                
                        Color finalEmission = Color.white * currentEmissionStrength;
                        _enemyMaterial.SetColor("_EmissionColor", finalEmission);
                    }
                }
                else
                {
                    _enemyMaterial.SetColor("_EmissionColor", Color.black);
                }
            }
        }
        else
        {
            // Disable collider and reset glow when sonar is inactive
            if(_detectionCollider != null)
            {
                _detectionCollider.enabled = false;
                _detectionCollider.radius = 0.5f;
            }
            
                _enemyMaterial.SetColor("_EmissionColor", Color.black);
            _enemyDetected = false;
            _trackedEnemy = null;
            _currentEmissionLerp = 0f;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Only detect the first enemy, ignore all others
        if(_trackedEnemy == null && other.CompareTag("Enemy"))
        {
            _trackedEnemy = other;
            _enemyDetected = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // Only respond to the tracked enemy leaving
        if(other == _trackedEnemy)
        {
            _enemyDetected = false;
            _trackedEnemy = null;
        }
    }
}
