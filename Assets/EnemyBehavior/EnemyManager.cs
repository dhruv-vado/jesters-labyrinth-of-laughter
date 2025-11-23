using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton

    public static EnemyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [HideInInspector]public Transform[] WayPoints;
    public bool _playerDetected = false;
    
    private List<Enemy> _enemies = new List<Enemy>();

    public void PlayerDetected(bool detected)
    {
        _playerDetected = detected;
    }
    
    public void RegisterEnemy(Enemy enemy)
    {
        if(!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }
    
    public void UnregisterEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
    
    public List<Enemy> GetAllEnemies()
    {
        return new List<Enemy>(_enemies);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in _enemies)
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        _enemies.Clear();
    }
}
