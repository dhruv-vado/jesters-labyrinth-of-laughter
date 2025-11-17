using UnityEngine;

public class EnemyDied : EnemyStatesBase
{
    public EnemyDied(Enemy enemy, EnemyStatesFactory enemyStatesFactory) : base(enemy, enemyStatesFactory)
    {
    }

    public override void EnterState()
    {
        
        Object.Destroy(Enemy.gameObject);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
