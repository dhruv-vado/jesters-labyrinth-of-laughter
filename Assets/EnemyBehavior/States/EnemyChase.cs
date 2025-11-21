using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyStatesBase
{
    public EnemyChase(Enemy enemy, EnemyStatesFactory enemyStatesFactory) : base(enemy, enemyStatesFactory)
    {
    }

    public override void EnterState()
    {
        if(!CanChase())
        {
            return;
        }

        Enemy.Agent.ResetPath();
        Enemy.Agent.speed = Enemy.ChaseSpeed;
        SetDestination(Enemy.Player.transform);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if(!CanChase())
        {
            return;
        }

        CheckAndSetDestination();
        Enemy.Agent.speed += Enemy.ChaseAcceleration * Time.deltaTime;
        SetDestination(Enemy.Player.transform);
    }

    private void CheckAndSetDestination()
    {
        
        //calc distance b/w current target
        float distance = Vector3.Distance(Enemy.transform.position, Enemy.Player.transform.position);

        if(distance <= Enemy.Agent.stoppingDistance + Enemy.extraChaseStopDistance)
        {
            //Agent reached the target
            Enemy.CurrentWayPoint = null;
            Enemy.Agent.ResetPath();
            //player dies
            Enemy.PlayerDied();
        }
    }
    
    private bool CanChase()
    {
        if(Enemy.Agent == null || !Enemy.Agent.enabled || !Enemy.Agent.isOnNavMesh)
        {
            return false;
        }

        if(Enemy.Player == null)
        {
            return false;
        }

        if(GameManager.Instance != null && !GameManager.Instance.IsPlayable)
        {
            return false;
        }

        return true;
    }

    private void SetDestination(Transform point)
    {
        if(point == null)
            return;

        SetDestination(point.position);
    }

    private void SetDestination(Vector3 point)
    {
        if(!CanChase())
        {
            if(Enemy.Agent != null && Enemy.Agent.enabled && Enemy.Agent.isOnNavMesh)
            {
                Enemy.Agent.ResetPath();
            }
            return;
        }

        Enemy.Agent.SetDestination(point);
    }
}
