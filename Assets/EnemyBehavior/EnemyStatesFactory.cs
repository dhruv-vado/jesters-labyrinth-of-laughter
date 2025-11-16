using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatesFactory : MonoBehaviour
{
    public Enemy Enemy;

    public EnemyStatesFactory(Enemy enemy)
    {
        Enemy = enemy;
    }

    public EnemyPatrol Patrol(){return new EnemyPatrol(Enemy,this);}

    public EnemyChase Chase(){return new EnemyChase(Enemy,this);}

}
