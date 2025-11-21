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
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _detectionMask;
    public bool _canSeePlayer = false;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        StartCoroutine(DetectionRoutine());
    }

    private IEnumerator DetectionRoutine()
    {
        float delay = 0.2f;

        WaitForSeconds wait = new WaitForSeconds(delay);
        _player = _enemy.Player;

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 direction = (_player.position - _detectionOrigin.position).normalized;

            if(Vector3.Angle(transform.forward, direction) < _detectionAngle/2)
            {
                float distance = Vector3.Distance(_detectionOrigin.position, _player.position);

                if(!Physics.Raycast(_detectionOrigin.position, direction, distance, _detectionMask))
                {
                    _canSeePlayer = true;
                    _enemy.PlayerFound(true);
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
            else
            {
                _canSeePlayer = false;
            }
        }
        else if(_canSeePlayer)
        {
            _canSeePlayer = false;
            _enemy.PlayerFound(false);
        }
    }
}
