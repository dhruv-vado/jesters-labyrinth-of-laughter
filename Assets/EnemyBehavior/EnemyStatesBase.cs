using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStatesBase
{
    public EnemyStatesBase(Enemy enemy, EnemyStatesFactory enemyStatesFactory)
    {
        Enemy = enemy;
        enemyStatesFactory = EStatesFactory;
    }


    public Enemy Enemy;

    public EnemyStatesFactory EStatesFactory;

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();

    public void SwitchStates(EnemyStatesBase newState)
    {
        //Exit the previous state
        Enemy.currentState.ExitState();

        //set new state
        Enemy.currentState = newState;

        //enter new state
        Enemy.currentState.EnterState();
    }

}
