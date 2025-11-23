using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Factory class - should NOT inherit from MonoBehaviour
public class EnemyStatesFactory
{
    public Enemy Enemy;

    public EnemyStatesFactory(Enemy enemy)
    {
        Enemy = enemy;
    }

    public EnemyPatrol Patrol(){return new EnemyPatrol(Enemy,this);}

    public EnemyChase Chase(){return new EnemyChase(Enemy,this);}

    public EnemyDied Died(){return new EnemyDied(Enemy,this);}

    public EnemyKill Kill(){return new EnemyKill(Enemy,this);}
}
