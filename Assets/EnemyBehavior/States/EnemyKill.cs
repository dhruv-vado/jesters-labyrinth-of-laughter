using UnityEngine;

public class EnemyKill : EnemyStatesBase
{
    public EnemyKill(Enemy enemy, EnemyStatesFactory enemyStatesFactory) : base(enemy, enemyStatesFactory)
    {
    }

    public override void EnterState()
    {
        EnemyManager.Instance.DestroyAllEnemies();
    }
    
    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {

    }
}
