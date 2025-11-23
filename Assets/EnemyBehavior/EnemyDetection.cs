using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDetection : MonoBehaviour
{
    public Transform _player;
    private Enemy _enemy;

    [SerializeField]private Transform _detectionOrigin;
    [Range(0,360)]
    [SerializeField]public float _detectionAngle;
    [SerializeField] public float _radius = 10f;
    [SerializeField] public float _ChaseRadius = 15f;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _detectionMask;
    public float _touchDetectionRange = 1f;
    public bool _canSeePlayer = false;
    private float rad;

    // Cache to reduce allocations
    private Collider[] _overlapResults = new Collider[32];
    private WaitForSeconds _detectionWait;
    private float _halfDetectionAngle;
    private Coroutine _detectionCoroutine;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        rad = _radius;
        _halfDetectionAngle = _detectionAngle / 2f;
        
        // Cache WaitForSeconds to avoid allocation
        _detectionWait = new WaitForSeconds(0.2f);
        
        _detectionCoroutine = StartCoroutine(DetectionRoutine());
    }

    private void OnDestroy()
    {
        // Stop coroutine to prevent leaks
        if(_detectionCoroutine != null)
        {
            StopCoroutine(_detectionCoroutine);
            _detectionCoroutine = null;
        }
        
        // Clean up cached wait object
        _detectionWait = null;
    }

    private IEnumerator DetectionRoutine()
    {
        // Wait for enemy to initialize
        yield return new WaitUntil(() => _enemy != null && _enemy.Player != null);
        _player = _enemy.Player;

        while(true)
        {
            yield return _detectionWait;
            
            // Skip if player or enemy is null (destroyed)
            if(_player == null || _enemy == null || _detectionOrigin == null)
            {
                yield break;
            }
            
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        // Use non-allocating version with cached array
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, rad, _overlapResults, _targetMask);

        if(hitCount > 0)
        {
            // Check if player is in range
            if(_player == null)
                return;
                
            Vector3 detectionPosition = _detectionOrigin.position;
            Vector3 playerPosition = _player.position;
            Vector3 direction = (playerPosition - detectionPosition);
            
            // Calculate horizontal distance (X and Z only)
            Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z);
            float distanceToPlayer = horizontalDirection.magnitude;
            
            // Normalize direction for angle check
            direction.Normalize();

            // Touch detection (fast check first)
            if(distanceToPlayer < _touchDetectionRange)
            {
                if(!_canSeePlayer)
                {
                    _canSeePlayer = true;
                    _enemy.PlayerFound(true);
                }
                return;
            }

            // Angle check
            float angleToPlayer = Vector3.Angle(transform.forward, direction);
            if(angleToPlayer < _halfDetectionAngle)
            {
                // Raycast check
                if(!Physics.Raycast(detectionPosition, direction, distanceToPlayer, _detectionMask))
                {
                    if(!_canSeePlayer)
                    {
                        _canSeePlayer = true;
                        rad = _ChaseRadius;
                        _enemy.PlayerFound(true);
                    }
                }
                else
                {
                    if(_canSeePlayer)
                    {
                        _canSeePlayer = false;
                        rad = _radius;
                        _enemy.PlayerFound(false);
                    }
                }
            }
            else
            {
                if(_canSeePlayer)
                {
                    _canSeePlayer = false;
                    rad = _radius;
                    _enemy.PlayerFound(false);
                }
            }
        }
        else if(_canSeePlayer)
        {
            _canSeePlayer = false;
            rad = _radius;
            _enemy.PlayerFound(false);
        }
    }
}
