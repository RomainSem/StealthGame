using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : MonoBehaviour
{
    #region Exposed

    [SerializeField] Transform[] _waypoints;
    [SerializeField] bool _backAndForth;
    [SerializeField] LayerMask _groundMask;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDetected = _player.GetComponent<PlayerDetected>();
        _playerTransform = _player.GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _agent.SetDestination(_waypoints[_currentPoint].position);
    }

    void Update()
    {
        Destination();
    }

    #endregion

    #region Methods

    private void Destination()
    {
        if (_playerDetected.IsPlayerVisible)
        {
            DetectionOfPlayer();
            if (_playerDetected.Shadow != null)
            {
                _shadowPosition = _playerDetected.Shadow.transform.position;
                gameObject.transform.LookAt(_shadowPosition);
            }
        }
        if (_isMovingToShadow)
        {
            if (_playerDetected.Shadow != null)
            {
                if (Vector3.Distance(transform.position, _playerDetected.Shadow.transform.position) <= 2f)
                {
                    Destroy(_playerDetected.Shadow);
                    _agent.ResetPath();
                    _isMovingToShadow = false;
                }
            }
        }
        else
        {
            if (_backAndForth)
            {
                if (_isGoing)
                {
                    GoToNextPoint();
                }
                else
                {
                    GoToPreviousPoint();
                }
            }
            else
            {
                GoToNextPoint();
            }
        }
    }

    private void DetectionOfPlayer()
    {
        Vector3 startingpoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
        Vector3 localDirection = transform.InverseTransformDirection(startingpoint);
        Vector3 _playerPosition = _playerTransform.position;
        if (Physics.Raycast(localDirection, Vector3.forward, out RaycastHit hit, 500 , _groundMask))
        {
            if (Vector3.Distance(transform.position, _playerPosition) < Vector3.Distance(transform.position, hit.point))
            {
                _agent.SetDestination(_playerPosition);
                _isMovingToShadow = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 zInfinity = new Vector3(transform.position.x, transform.position.y, transform.position.z + 500);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, zInfinity);
    }

    private void GoToNextPoint()
    {
        if (_agent.remainingDistance <= 1)
        {
            _currentPoint++;
            if (_currentPoint >= _waypoints.Length)
            {
                if (_backAndForth)
                {
                    _isGoing = false;
                    _currentPoint = _waypoints.Length - 1;
                }
                else
                {
                    _currentPoint = 0;
                }
            }
            _agent.SetDestination(_waypoints[_currentPoint].position);
        }
    }

    private void GoToPreviousPoint()
    {
        if (_agent.remainingDistance <= 1)
        {
            _currentPoint--;

            if (_currentPoint < 0)
            {
                if (_backAndForth)
                {
                    _isGoing = true;
                    _currentPoint = 0;
                }
                else
                {
                    _currentPoint = _waypoints.Length - 1;
                }
            }
            _agent.SetDestination(_waypoints[_currentPoint].position);
        }
    }

    #endregion

    #region Private & Protected

    NavMeshAgent _agent;
    int _currentPoint = 0;
    bool _isGoing;
    bool _isMovingToShadow = false;
    PlayerDetected _playerDetected;
    Transform _playerTransform;
    GameObject _player;
    Vector3 _shadowPosition;

    #endregion
}
