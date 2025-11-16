using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Fields")]
    private bool _playerHit;
    public Transform _player;
    private Enemy _enemy;
    private bool _playerDefined = false;

    [SerializeField]private Transform _detectionOrigin;
    [SerializeField]private float _detectionAngle;

    [Header("RayCheck")]
    [SerializeField] private LayerMask _detectionMask;

    [SerializeField] private bool _playerFound = false;

    [Header("Editor")]
    public SphereCollider _sphereCollider;
    public float ColliderRadius { get { return _sphereCollider.radius; } }
    public Transform DetectionOrigin { get { return _detectionOrigin; } }
    public float DetectionAngle { get { return _detectionAngle; } }

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void Update()
    {
        DetectAngle(); 
    }

    private void DetectAngle()
    {
        if(!_playerHit || _playerFound)
            return;

        _player = _enemy.Player;

        Vector3 direction = _player.position - _detectionOrigin.position;
        
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
        Vector3 flatForward = new Vector3(_detectionOrigin.forward.x, 0, _detectionOrigin.forward.z).normalized;
        
        float targetAngle = _detectionAngle * 0.5f;
        float angle = Vector3.Angle(flatDirection, flatForward);

        if(angle < targetAngle)
        {
            if(RayCheck())
            {
                _playerFound = true;
                _enemy.PlayerFound(true);
            }
        }
    }

    private bool RayCheck()
    {
        Ray ray = new Ray(_detectionOrigin.position, _detectionOrigin.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, 500, _detectionMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_playerFound == true)
            {
                _enemy.PlayerFound(false);
            }
            _playerHit = false;
            _playerFound = false;
        }
    }
}
