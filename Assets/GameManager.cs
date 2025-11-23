using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum GameState
{
    Initializing,
    Playing,
    Paused,
    GameOver,
    Won
}

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Game Settings")]
    [SerializeField] private GameState _currentState = GameState.Initializing;
    [SerializeField] private int _numberOfEnemies = 3;
    [SerializeField] private float _minSpawnDistanceFromPlayer = 5f;

    [Header("References")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private BuildNavMesh _navMesh;
    [SerializeField] private AudioSource _bgSound;
    [SerializeField] private AudioClip _bgAmbience;
    [SerializeField] private AudioClip _bgChase;
    [SerializeField] private TextMeshProUGUI _enemyCountText;


    [Header("Game Events")]
    public UnityEvent OnGameStart;
    public UnityEvent OnPlayerCaught;
    public UnityEvent OnWon;
    public UnityEvent OnPause;
    public UnityEvent OnResume;

    private EnemyManager _enemyManager;

    private int _currentEnemyCount;

    private List<Enemy> _spawnedEnemies = new List<Enemy>();
    private Transform _playerTransform;

    public bool IsPlayable = true;
    public bool IsPaused = false;

    public GameState CurrentState => _currentState;
    public int ActiveEnemies => _spawnedEnemies.Count;

    private void Start()
    {
        StartCoroutine(InitializeGame());
        _enemyManager = EnemyManager.Instance;
    }

    private IEnumerator InitializeGame()
    {
        _currentState = GameState.Initializing;
        
        yield return new WaitUntil(() => EnemyManager.Instance.WayPoints != null && EnemyManager.Instance.WayPoints.Length > 0);
        
        yield return new WaitForSeconds(0.5f);
        
        yield return new WaitUntil(() => PlayerManager.Instance != null);
        yield return new WaitForSeconds(0.5f);
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            _playerTransform = player.transform;
        }

        _navMesh.Build();
        
        PlayerManager.Instance.Spawn();
        Cursor.lockState = CursorLockMode.Locked;
        SpawnEnemies();

        _currentEnemyCount = _numberOfEnemies;
        _enemyCountText.text = "Clowns Left: " + _currentEnemyCount.ToString(); 

        _currentState = GameState.Playing;
        OnGameStart?.Invoke();

        _bgSound.clip = _bgAmbience;
        _bgSound.Play();
    }

    public void SpawnEnemies()
    {
        if(_enemyPrefab == null)
            return;

        if(EnemyManager.Instance.WayPoints == null || EnemyManager.Instance.WayPoints.Length == 0)
            return;

        foreach(Enemy enemy in _spawnedEnemies)
        {
            if(enemy != null)
                Destroy(enemy.gameObject);
        }
        _spawnedEnemies.Clear();  //reset enemies before spawning

        List<Transform> validSpawnPoints = GetValidSpawnPoints();

        if(validSpawnPoints.Count == 0)
            validSpawnPoints = new List<Transform>(EnemyManager.Instance.WayPoints);

        int enemiesToSpawn = Mathf.Min(_numberOfEnemies, validSpawnPoints.Count);
        
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
            validSpawnPoints.Remove(spawnPoint);

            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = 1f;

            Enemy newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.InitializeStates();
            _spawnedEnemies.Add(newEnemy);
        }
    }

    private List<Transform> GetValidSpawnPoints()
    {
        List<Transform> validPoints = new List<Transform>();

        if(_playerTransform == null)
        {
            foreach(Transform waypoint in EnemyManager.Instance.WayPoints)
            {
                if(waypoint != null)
                    validPoints.Add(waypoint);
            }
            return validPoints;
        }

        foreach(Transform waypoint in EnemyManager.Instance.WayPoints)
        {
            if(waypoint == null) continue;

            float distanceToPlayer = Vector3.Distance(waypoint.position, _playerTransform.position);
            if(distanceToPlayer >= _minSpawnDistanceFromPlayer)
            {
                validPoints.Add(waypoint);
            }
        }

        return validPoints;
    }

    public void Update()
    {
        if(InputManager.Instance.PauseGame())
        {
            if(IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void UpdateEnemyCount()
    {
        _currentEnemyCount--;
        _enemyCountText.text = "Clowns Left: " + _currentEnemyCount.ToString(); 
    }

    public void ChasingPlayer()
    {
        _bgSound.clip = _bgChase;
        _bgSound.Play();
    }

    public void PlayingAmbience()
    {
        _bgSound.clip = _bgAmbience;
        _bgSound.Play();
    }

    public void PlayerCaught()
    {
        if(_currentState != GameState.Playing) return;

        PlayerManager.Instance.PlayerCaught();
        IsPlayable = false;
        _currentState = GameState.GameOver;
        Cursor.lockState = CursorLockMode.None;
        OnPlayerCaught?.Invoke();
    }

    public void PlayerEscaped()
    {
        if(_currentState != GameState.Playing) return;

        IsPlayable = false;
        _enemyManager.DestroyAllEnemies();
        _spawnedEnemies.Clear();
        _currentState = GameState.Won;
        Cursor.lockState = CursorLockMode.None;
        OnWon?.Invoke();
        Time.timeScale = 0.01f;
    }

    public void PauseGame()
    {
        if(_currentState == GameState.Playing)
        {
            IsPlayable = false;
            _currentState = GameState.Paused;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            IsPaused = true;
            OnPause?.Invoke();
        }
    }

    public void ResumeGame()
    {
        if(_currentState == GameState.Paused)
        {
            IsPlayable = true;
            _currentState = GameState.Playing;
            Cursor.lockState = CursorLockMode.Locked;  
            Time.timeScale = 1f;
            IsPaused = false;
            OnResume?.Invoke();
        }
    }

    public void RestartGame()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.Level);
    }

    private void OnDestroy()
    {
        IsPlayable = true;
        Time.timeScale = 1f;
    }
}

