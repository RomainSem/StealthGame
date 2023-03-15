using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : MonoBehaviour
{
    #region Exposed

    [SerializeField] Transform[] _waypoints;
    [SerializeField] bool _backAndForth;

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
            PlayerGetsDetected();
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

    private void PlayerGetsDetected()
    {
        Vector3 _playerPosition = _playerTransform.position;
        _agent.SetDestination(_playerPosition);
        _isMovingToShadow = true;
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

    #endregion
}
