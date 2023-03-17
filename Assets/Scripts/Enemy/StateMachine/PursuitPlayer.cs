using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PursuitPlayer : StateMachineBehaviour
{

    [SerializeField] GameObject _playerShadow;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDetectedScript = _player.GetComponent<PlayerDetected>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy = animator.gameObject;
        _agent = _enemy.GetComponent<NavMeshAgent>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (_playerDetectedScript.IsDetectedByCamera)
        {
            if (_shadow != null)
            {
                _shadowPosition = _shadow.transform.position;
                _agent.SetDestination(_shadowPosition);
                _enemy.transform.LookAt(_shadowPosition);
            }
            else if (_playerDetectedScript.Shadow != null)
            {
                _agent.SetDestination(_playerDetectedScript.Shadow.transform.position);
                _enemy.transform.LookAt(_player.transform.position);
            }
            else
            {
                _agent.SetDestination(_player.transform.position);
                _enemy.transform.LookAt(_player.transform.position);
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(_enemy.transform.position, _player.transform.position - _enemy.transform.position, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                _enemy.transform.LookAt(_player.transform.position);
                _agent.SetDestination(_player.transform.position);
                if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 1f)
                {
                    _playerDetectedScript.IsPlayerVisible = true;
                    _isPlayerCollided = true;
                    animator.SetBool("IsPlayerCollided", _isPlayerCollided);
                }
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                _playerDetectedScript.IsPlayerVisible = false;
                Vector3 playerPosLastSeen = _player.transform.position;
                if (_shadow != null)
                {
                    Destroy(_shadow);
                    _shadow = Instantiate(_playerShadow, playerPosLastSeen, Quaternion.identity);
                    _enemy.transform.LookAt(playerPosLastSeen);
                    _agent.SetDestination(playerPosLastSeen);
                    if (Vector3.Distance(_enemy.transform.position, playerPosLastSeen) <= 1f)
                    {
                        _isShadowPlayerCollided = true;
                        animator.SetBool("IsShadowPlayerCollided", _isShadowPlayerCollided);
                    }
                }
                else if (_playerDetectedScript.Shadow != null)
                {
                    Destroy(_playerDetectedScript.Shadow);
                    _shadow = Instantiate(_playerShadow, playerPosLastSeen, Quaternion.identity);
                    _enemy.transform.LookAt(playerPosLastSeen);
                    _agent.SetDestination(playerPosLastSeen);
                    if (Vector3.Distance(_enemy.transform.position, playerPosLastSeen) <= 1f)
                    {
                        _isShadowPlayerCollided = true;
                        animator.SetBool("IsShadowPlayerCollided", _isShadowPlayerCollided);
                    }
                }
            }
        }
    }


    PlayerDetected _playerDetectedScript;
    GameObject _enemy;
    NavMeshAgent _agent;
    GameObject _player;
    GameObject _shadow;
    Vector3 _shadowPosition;
    bool _isPlayerCollided;
    bool _isShadowPlayerCollided;
}



