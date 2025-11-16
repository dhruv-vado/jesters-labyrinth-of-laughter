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
        Enemy.Agent.ResetPath();
        Enemy.Agent.speed = Enemy.ChaseSpeed;
        SetDestination(Enemy.Player.transform);
        Debug.Log("Chase State Started");
    }

    public override void ExitState()
    {
        Debug.Log("Chase State Exited");
    }

    public override void UpdateState()
    {
        CheckAndSetDestination();
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
            Time.timeScale = 0f;
            Debug.Log("Player Died.");
        }
    }
    
    private void SetDestination(Transform point) => Enemy.Agent.SetDestination(point.position);

    private void SetDestination(Vector3 point) => Enemy.Agent.SetDestination(point);
}
