using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarWave : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private float _initialRadius = 0.5f;
    [SerializeField] private float _maxRadius = 30f;
    [SerializeField] private float _expansionSpeed = 15f;
    [SerializeField] private float _maxIntensity = 2f;
    [SerializeField] private float _intensityFadeStartPercent = 0.7f;
    [SerializeField] private float _fadeOutDuration = 2f;
    [SerializeField] private float _cooldown = 2.5f;
    [SerializeField] private Light _light;

    [Header("Enemy Glow")]
    [SerializeField] private LayerMask _enemyLayerMask = ~0;
    [SerializeField] private Color _glowColor = Color.red;
    [SerializeField] private float _emissionStrength = 5f;
    [SerializeField] private SphereCollider _detectionCollider;

    private InputManager _inputManager;
    private Rigidbody _rigidbody;

    private class EnemyGlowData
    {
        public Renderer Renderer;
        public Material OriginalMaterial;
        public Material GlowMaterial;
    }

    private Dictionary<Enemy, EnemyGlowData> _glowingEnemies = new Dictionary<Enemy, EnemyGlowData>();
    private float _currentRadius;
    private bool _isActive = false;
    private bool _isFadingOut = false;
    private float _lastSonarTime = -999f;

    private void Start()
    {
        _currentRadius = _initialRadius;
        _light.range = _initialRadius;
        _light.intensity = 0f;
        _inputManager = InputManager.Instance;
        
        if(_detectionCollider == null)
        {
            _detectionCollider = GetComponent<SphereCollider>();
        }

        if(_detectionCollider == null)
        {
            _detectionCollider = gameObject.AddComponent<SphereCollider>();
        }

        _detectionCollider.isTrigger = true;
        _detectionCollider.radius = _initialRadius;
        _detectionCollider.enabled = false;

        _rigidbody = GetComponent<Rigidbody>();
        if(_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
    }
    
    private void Update()
    {
        float timeSinceLastSonar = Time.time - _lastSonarTime;
        
        if(_inputManager.Sonar() && timeSinceLastSonar >= _cooldown && !_isActive)
        {
            StartSonarPulse();
        }
        
        if(_isActive)
        {
            if(!_isFadingOut)
            {
                _currentRadius += _expansionSpeed * Time.deltaTime;
                _light.range = _currentRadius;
                
                if(_detectionCollider != null)
                {
                    _detectionCollider.radius = _currentRadius;
                }
                
                float expansionPercent = _currentRadius / _maxRadius;
                
                if(expansionPercent >= _intensityFadeStartPercent)
                {
                    float fadeProgress = (expansionPercent - _intensityFadeStartPercent) / (1f - _intensityFadeStartPercent);
                    _light.intensity = _maxIntensity * (1f - fadeProgress);
                }
                else
                {
                    _light.intensity = _maxIntensity;
                }

                UpdateEnemyEmission();
                
                if(_currentRadius >= _maxRadius)
                {
                    _isFadingOut = true;
                    ResetSonar();
                }
            }
        }
        else
        {
            UpdateEnemyEmission();
        }
    }
    
    private void StartSonarPulse()
    {
        _isActive = true;
        _isFadingOut = false;
        _currentRadius = _initialRadius;
        _light.range = _initialRadius;
        _light.intensity = _maxIntensity;
        _lastSonarTime = Time.time;
        
        if(_detectionCollider != null)
        {
            _detectionCollider.enabled = true;
            _detectionCollider.radius = _initialRadius;
        }
        
    }
    
    private void ResetSonar()
    {
        _isActive = false;
        _isFadingOut = false;
        _currentRadius = _initialRadius;
        _light.range = _initialRadius;
        _light.intensity = 0f;
        
        if(_detectionCollider != null)
        {
            _detectionCollider.enabled = false;
            _detectionCollider.radius = _initialRadius;
        }

        RestoreAllEnemyMaterials();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(IsEnemyCollider(other))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null)
            {
                MakeEnemyGlow(enemy);
            }
        }
    }

    private bool IsEnemyCollider(Collider other)
    {
        bool layerMatch = ((_enemyLayerMask.value & (1 << other.gameObject.layer)) != 0);
        bool tagMatch = other.CompareTag("Enemy");
        return layerMatch || tagMatch;
    }
    
    private void MakeEnemyGlow(Enemy enemy)
    {
        Renderer renderer = enemy._renderer != null ? enemy._renderer : enemy.GetComponentInChildren<Renderer>();
        if(renderer == null)
            return;
        
        Material originalMaterial = renderer.material;
        Material glowMaterial = new Material(originalMaterial);
        
        if(glowMaterial.HasProperty("_EmissionColor"))
        {
            glowMaterial.EnableKeyword("_EMISSION");
            glowMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }
        
        renderer.material = glowMaterial;
        
        _glowingEnemies[enemy] = new EnemyGlowData
        {
            Renderer = renderer,
            OriginalMaterial = originalMaterial,
            GlowMaterial = glowMaterial
        };
        
        UpdateEnemyEmission();
    }
    
    private void UpdateEnemyEmission()
    {
        if(_glowingEnemies.Count == 0)
            return;
        
        float intensityFactor = Mathf.Approximately(_maxIntensity, 0f) ? 0f : Mathf.Clamp01(_light.intensity / _maxIntensity);
        Color emissionColor = _glowColor * (_emissionStrength * intensityFactor);
        
        foreach(var data in _glowingEnemies.Values)
        {
            if(data?.GlowMaterial == null)
                continue;
            
            if(data.GlowMaterial.HasProperty("_EmissionColor"))
            {
                data.GlowMaterial.SetColor("_EmissionColor", emissionColor);
            }
        }
    }
    
    private void RestoreAllEnemyMaterials()
    {
        foreach(var kvp in _glowingEnemies)
        {
            RestoreEnemyMaterial(kvp.Value);
        }
        _glowingEnemies.Clear();
    }
    
    private void RestoreEnemyMaterial(EnemyGlowData data)
    {
        if(data == null || data.Renderer == null || data.OriginalMaterial == null)
            return;
        
        data.Renderer.material = data.OriginalMaterial;
        
        if(data.GlowMaterial != null)
        {
            Destroy(data.GlowMaterial);
        }
    }

    private void OnDestroy()
    {
        RestoreAllEnemyMaterials();
    }
}

