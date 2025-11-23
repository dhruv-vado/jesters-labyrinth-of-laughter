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

    private Collider[] _overlapResults = new Collider[32];
    private WaitForSeconds _detectionWait;
    private float _halfDetectionAngle;
    private Coroutine _detectionCoroutine;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        rad = _radius;
        _halfDetectionAngle = _detectionAngle / 2f;
        
        _detectionWait = new WaitForSeconds(0.2f);
        
        _detectionCoroutine = StartCoroutine(DetectionRoutine());
    }

    private void OnDestroy()
    {
        if(_detectionCoroutine != null)
        {
            StopCoroutine(_detectionCoroutine);
            _detectionCoroutine = null;
        }
        
        _detectionWait = null;
    }

    private IEnumerator DetectionRoutine()
    {
        yield return new WaitUntil(() => _enemy != null && _enemy.Player != null);
        _player = _enemy.Player;

        while(true)
        {
            yield return _detectionWait;
            
            if(_player == null || _enemy == null || _detectionOrigin == null)
            {
                yield break;
            }
            
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, rad, _overlapResults, _targetMask);

        if(hitCount > 0)
        {
            if(_player == null)
                return;
                
            Vector3 detectionPosition = _detectionOrigin.position;
            Vector3 playerPosition = _player.position;
            Vector3 direction = (playerPosition - detectionPosition);
            
            Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z);
            float distanceToPlayer = horizontalDirection.magnitude;
            
            direction.Normalize();

            if(distanceToPlayer < _touchDetectionRange)
            {
                if(!_canSeePlayer)
                {
                    _canSeePlayer = true;
                    rad = _ChaseRadius;
                    _enemy.PlayerFound(true);
                }
                return;
            }

            if(_canSeePlayer)
            {
                return;
            }

            float angleToPlayer = Vector3.Angle(transform.forward, direction);
            if(angleToPlayer < _halfDetectionAngle)
            {
                if(!Physics.Raycast(detectionPosition, direction, distanceToPlayer, _detectionMask))
                {
                    _canSeePlayer = true;
                    rad = _ChaseRadius;
                    _enemy.PlayerFound(true);
                }
            }
        }
        else if(_canSeePlayer)
        {
            _canSeePlayer = false;
            rad = _radius;
            _enemy.PlayerFound(false);
            Debug.Log("Player is out of chase range");
        }
    }
}
