using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyStatesBase
{
    private Coroutine patrolCoroutine;
    public EnemyPatrol(Enemy enemy, EnemyStatesFactory enemyStatesFactory) : base(enemy, enemyStatesFactory)
    {

    }

    public override void EnterState()
    {
        Enemy.Agent.speed = Enemy.PatrolSpeed;
        SetRandomPoint();
        SetDestination(Enemy.CurrentWayPoint);
        patrolCoroutine = Enemy.StartCoroutine(PatrolRoutine());
    }

    public override void ExitState()
    {
        if (patrolCoroutine != null)
        {
            Enemy.StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    public override void UpdateState()
    {
        CheckAndSetDestination();
    }

    private void CheckAndSetDestination()
    {
        if(Enemy.Agent.hasPath && Enemy.CurrentWayPoint != null)
        {
            //calc distance b/w current target
            float distance = Vector3.Distance(Enemy.transform.position, Enemy.CurrentWayPoint.position);

            if(distance <= Enemy.Agent.stoppingDistance + Enemy.extraPatrolStopDistance)
            {
                //Agent reached the target
                Enemy.CurrentWayPoint = null;
                Enemy.Agent.ResetPath();
                this.EnterState();
            }
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 40f));
            Enemy.PlayPatrolAudio();
        }
    }

    private void SetRandomPoint()
    {
        int randomPoint = Random.Range(0, Enemy.WayPoints.Length);
        Transform target = Enemy.WayPoints[randomPoint];

        if(Enemy.CurrentWayPoint)
        {
            if(target == Enemy.CurrentWayPoint)
            {
                if(randomPoint == Enemy.WayPoints.Length - 1)
                    randomPoint--;
                else
                    randomPoint++;
            }
        }

        target = Enemy.WayPoints[randomPoint];

        Enemy.CurrentWayPoint = target;
    }

    private void SetDestination(Transform point) => Enemy.Agent.SetDestination(point.position);

    private void SetDestination(Vector3 point) => Enemy.Agent.SetDestination(point);

}
