using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
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

        if (_playerDetectedScript.IsDetected)
        {
            if (_shadow != null)
            {
                _shadowPosition = _shadow.transform.position;
                _agent.SetDestination(_shadowPosition);
                _enemy.transform.LookAt(_shadowPosition);
            }
            else if (_playerDetectedScript.Shadow != null)
            {
                _shadowPosition = _playerDetectedScript.Shadow.transform.position;
                _agent.SetDestination(_shadowPosition);
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
                    //_playerDetectedScript.IsPlayerVisible = true;
                    _isPlayerCollided = true;
                    animator.SetBool("IsPlayerCollided", _isPlayerCollided);
                }
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                Vector3 _playerPosLastSeen = _player.transform.position;

                //_playerDetectedScript.IsPlayerVisible = false;
                if (_shadow == null && _playerDetectedScript.Shadow == null && _isShadowInstantiated == false)
                {
                    //CreateShadow(_playerPosLastSeen);
                    _playerPosLastSeen = _player.transform.position;
                    _shadow = Instantiate(_playerShadow, _playerPosLastSeen, Quaternion.identity);
                    _isShadowInstantiated = true;
                }
                else
                {
                    Destroy(_shadow);
                    Destroy(_playerDetectedScript.Shadow);
                }
                _enemy.transform.LookAt(_playerPosLastSeen);
                _agent.SetDestination(_playerPosLastSeen);
                if (Vector3.Distance(_enemy.transform.position, _playerPosLastSeen) <= 1f)
                {
                    _isShadowPlayerCollided = true;
                    animator.SetBool("IsShadowPlayerCollided", _isShadowPlayerCollided);
                }
            }
        }
    }

    //private void CreateShadow(Vector3 position)
    //{
    //    _playerPosLastSeen = _player.transform.position;
    //    _shadow = Instantiate(_playerShadow, position, Quaternion.identity);
    //    Debug.Log("VARIABLE : " + position);
    //    Debug.Log("PLAYER : " + _player.transform.position);
    //    _isShadowInstantiated = true;
    //}


    PlayerDetected _playerDetectedScript;
    GameObject _enemy;
    NavMeshAgent _agent;
    GameObject _player;
    GameObject _shadow;
    Vector3 _shadowPosition;
    //Vector3 _playerPosLastSeen;
    bool _isPlayerCollided;
    bool _isShadowPlayerCollided;
    bool _isShadowInstantiated;
}



