using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Agent")]
    [HideInInspector]public UnityEngine.AI.NavMeshAgent Agent;
    public float PatrolSpeed = 5f;
    public float ChaseSpeed = 8f;
    public float ChaseAcceleration = 1f;
    public Animator _animator;

    [Header("References")]
    [HideInInspector]public Transform[] WayPoints;
    public Transform CurrentWayPoint;
    public float extraPatrolStopDistance = 1.5f;
    public float extraChaseStopDistance = 0.5f;
    public bool _playerFound;
    public Renderer _renderer;
    public EnemySounds _enemySounds;

    [Header("States")]
    private EnemyStatesFactory _enemyStatesFactory;

    private EnemyStatesBase _enemyStatesBase;

    public EnemyStatesBase currentState{get{return _enemyStatesBase;} set {_enemyStatesBase = value;}}

    public Transform Player {get; set;}  //basically get set hides this in inspector

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void InitializeStates()
    {
        //assigning waypoints
        WayPoints = EnemyManager.Instance.WayPoints;

        Player = GameObject.FindGameObjectWithTag("Player").transform;

        //creating new instance of the states
        _enemyStatesFactory = new EnemyStatesFactory(this);
        _enemyStatesBase = _enemyStatesFactory.Patrol();
        _enemyStatesBase.EnterState();
        
        if(EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterEnemy(this);
        }
    }
    
    private void OnDestroy()
    {
        // Unregister from EnemyManager
        if(EnemyManager.Instance != null)
        {
            EnemyManager.Instance.UnregisterEnemy(this);
        }
    }

    private void Update()
    {
        //update the state
        _enemyStatesBase.UpdateState();
    }

    private bool _isChaseAudioPlaying = false;

    public void PlayPatrolAudio()
    {
        _enemySounds.PlayPatrolAudio();
    }

    public void PlayerFound(bool found)
    {
        if(_playerFound == found)
        {
            return;
        }

        _playerFound = found;

        if(found)
        {
            _enemyStatesBase.SwitchStates(_enemyStatesFactory.Chase());

            if(!_isChaseAudioPlaying && _enemySounds != null)
            {
                _enemySounds.PlayChaseAudio();
                _isChaseAudioPlaying = true;
            }

            GameManager.Instance.ChasingPlayer();
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _enemyStatesBase.SwitchStates(_enemyStatesFactory.Patrol());

            if(_enemySounds != null)
            {
                _enemySounds.PlayPatrolAudio();
            }

            _isChaseAudioPlaying = false;
            _animator.SetBool("isRunning", false);
            GameManager.Instance.PlayingAmbience();
        }
    }

    public void PlayerDied()
    {
        GameManager.Instance.PlayerCaught();
        _enemyStatesBase.SwitchStates(_enemyStatesFactory.Kill());
    }

    public void EnemyDied()
    {
        _enemySounds.PlayDeadAudio();
        _enemyStatesBase.SwitchStates(_enemyStatesFactory.Died());
    }
}
