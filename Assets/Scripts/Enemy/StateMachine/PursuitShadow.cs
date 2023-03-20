using UnityEngine;
using UnityEngine.AI;

public class PursuitShadow : StateMachineBehaviour
{
    [SerializeField] GameObject _playerShadow;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy = animator.gameObject;
        _agent = _enemy.GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDetectedScript = _player.GetComponent<PlayerDetected>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RaycastHit hit;
        if (Physics.Raycast(_enemy.transform.position, _player.transform.position - _enemy.transform.position, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                _playerDetectedScript.IsEnemyRayHittingPlayer = true;
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                Vector3 _playerPosLastSeen = _player.transform.position;

                _playerDetectedScript.IsEnemyRayHittingPlayer = false;
                if (_shadow == null && _playerDetectedScript.Shadow == null && _isShadowInstantiated == false)
                {
                    _playerPosLastSeen = _player.transform.position;
                    _shadow = Instantiate(_playerShadow, _playerPosLastSeen, Quaternion.identity);
                    Debug.Log(_shadow.transform.position);
                    _isShadowInstantiated = true;
                }
                else
                {
                    Destroy(_playerDetectedScript.Shadow);
                    Debug.Log("DELETE SHADOW");
                }
                _enemy.transform.LookAt(_shadow.transform.position);
                _agent.SetDestination(_shadow.transform.position);

                if (Vector3.Distance(_enemy.transform.position, _shadow.transform.position) <= 1f)
                {
                    _isShadowPlayerCollided = true;
                    animator.SetBool("IsShadowFound", _isShadowPlayerCollided);
                }
            }

            animator.SetBool("IsEnemyRayHittingPlayer", _playerDetectedScript.IsEnemyRayHittingPlayer);
        }
    }

    PlayerDetected _playerDetectedScript;
    NavMeshAgent _agent;
    GameObject _enemy;
    GameObject _player;
    GameObject _shadow;
    bool _isShadowPlayerCollided;
    bool _isShadowInstantiated;

}
