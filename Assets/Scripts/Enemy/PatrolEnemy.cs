using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : MonoBehaviour
{
    #region Exposed

    [SerializeField] Transform[] _waypoints;
    [SerializeField] bool _backAndForth;
    [Header("Player")]
    [SerializeField] GameObject _player;
    [SerializeField] Transform _playerTransform;
    [SerializeField] PlayerDetected _playerDetected;
    [Header("Raycast")]
    [SerializeField] LayerMask _groundMask;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        //_agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //_agent.SetDestination(_waypoints[_currentPoint].position);
    }

    void Update()
    {
       // Destination();
    }

    #endregion

    #region Methods

    //private void Destination()
    //{
    //    if (_playerDetected.IsPlayerVisible)
    //    {
    //        DetectionOfPlayer();
    //        if (_playerDetected.Shadow != null)
    //        {
    //            _shadowPosition = _playerDetected.Shadow.transform.position;
    //            gameObject.transform.LookAt(_shadowPosition);
    //        }
    //    }
    //    if (_isMovingToShadow)
    //    {
    //        if (_playerDetected.Shadow != null)
    //        {
    //            if (Vector3.Distance(transform.position, _playerDetected.Shadow.transform.position) <= 2f)
    //            {
    //                Destroy(_playerDetected.Shadow);
    //                _agent.ResetPath();
    //                _isMovingToShadow = false;
    //            }
    //        }
    //    }

    //private void DetectionOfPlayer()
    //{
    //    Vector3 startingpoint = transform.position;
    //    //Vector3 localDirection = transform.InverseTransformDirection(startingpoint);
    //    Vector3 directionToPlayer = _playerTransform.position - startingpoint;
    //    if (Physics.Raycast(startingpoint, directionToPlayer, out RaycastHit hit, 500 , _groundMask))
    //    {
    //        Debug.Log(hit.collider.name);
    //        if (Vector3.Distance(startingpoint, _playerTransform.position) < Vector3.Distance(startingpoint, hit.point))
    //        {

    //            //_agent.SetDestination(_playerTransform.position);
    //            //_isMovingToShadow = true;
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        if (_playerTransform != null)
        {
            Gizmos.color = Color.red;
            Vector3 directionToPlayer = _playerTransform.position - transform.position;
            Gizmos.DrawLine(transform.position, transform.position + directionToPlayer);
        }
    }

    #endregion

    #region Private & Protected

    //NavMeshAgent _agent;
    //int _currentPoint = 0;
    //bool _isGoing;
    //bool _isMovingToShadow = false;
    //Vector3 _shadowPosition;

    public Transform[] Waypoints { get => _waypoints; private set => _waypoints = value; }
    public bool BackAndForth { get => _backAndForth; private set => _backAndForth = value; }
    //public LayerMask GroundMask { get => _groundMask; set => _groundMask = value; }


    #endregion
}
