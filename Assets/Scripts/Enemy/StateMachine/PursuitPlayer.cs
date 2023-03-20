using UnityEngine;
using UnityEngine.AI;

public class PursuitPlayer : StateMachineBehaviour
{
    [SerializeField] GameObject _playerShadow;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDetectedScript = _player.GetComponent<PlayerDetected>();
        _enemy = animator.gameObject;
        _agent = _enemy.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //if (_playerDetectedScript.IsDetectedByCam)
        //{
        //    if (_shadow != null)
        //    {
        //        _shadowPosition = _shadow.transform.position;
        //        _agent.SetDestination(_shadowPosition);
        //        _enemy.transform.LookAt(_shadowPosition);
        //    }
        //    else if (_playerDetectedScript.Shadow != null)
        //    {
        //        _shadowPosition = _playerDetectedScript.Shadow.transform.position;
        //        _agent.SetDestination(_shadowPosition);
        //        _enemy.transform.LookAt(_player.transform.position);
        //    }
        //    else
        //    {
        //        _agent.SetDestination(_player.transform.position);
        //        _enemy.transform.LookAt(_player.transform.position);
        //    }
        //}

        RaycastHit hit;
        if (Physics.Raycast(_enemy.transform.position, _player.transform.position - _enemy.transform.position, out hit))
        {
            if (hit.collider.gameObject.tag == "Player" && _playerDetectedScript.IsDetectedByEnemy)
            {
                _playerDetectedScript.IsEnemyRayHittingPlayer = true;
                _enemy.transform.LookAt(_player.transform.position);
                _agent.SetDestination(_player.transform.position);
                if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 1f)
                {
                    _isPlayerFound = true;
                    animator.SetBool("IsPlayerFound", _isPlayerFound);
                }
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                //Vector3 _playerPosLastSeen = _player.transform.position;

                _playerDetectedScript.IsEnemyRayHittingPlayer = false;
                //if (_shadow == null && _playerDetectedScript.Shadow == null && _isShadowInstantiated == false)
                //{
                //    _playerPosLastSeen = _player.transform.position;
                //    _shadow = Instantiate(_playerShadow, _playerPosLastSeen, Quaternion.identity);
                //    Debug.Log(_shadow.transform.position);
                //    _isShadowInstantiated = true;
                //}
                //else
                //{
                //    Destroy(_playerDetectedScript.Shadow);
                //}
                //_enemy.transform.LookAt(_playerPosLastSeen);
                //_agent.SetDestination(_playerPosLastSeen);
                //if (Vector3.Distance(_enemy.transform.position, _playerPosLastSeen) <= 1f)
                //{
                //    _isShadowPlayerCollided = true;
                //    animator.SetBool("IsShadowPlayerCollided", _isShadowPlayerCollided);
                //}
            }
            animator.SetBool("IsEnemyRayHittingPlayer", _playerDetectedScript.IsEnemyRayHittingPlayer);
        }
    }


    PlayerDetected _playerDetectedScript;
    GameObject _enemy;
    NavMeshAgent _agent;
    GameObject _player;
    GameObject _shadow;
    Vector3 _shadowPosition;
    bool _isPlayerFound;
    bool _isShadowPlayerCollided;
    bool _isShadowInstantiated;
}



