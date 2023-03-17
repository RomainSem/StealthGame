using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Hesitating : StateMachineBehaviour
{

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
        _patrolEnemyScript = _enemy.GetComponent<PatrolEnemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        RaycastHit hit;
        if (Physics.Raycast(_enemy.transform.position, _player.transform.position - _enemy.transform.position, out hit))
        {
            if (hit.collider.gameObject.tag == "Player" && _playerDetectedScript.IsPlayerVisible)
            {
                timer += Time.deltaTime;
            }
            else /*if (hit.collider.gameObject.tag == "Ground")*/
            {
                timer = 0f;
                _playerDetectedScript.IsPlayerVisible = false;
            }
            animator.SetFloat("TimerToStartPursuit", timer);
        }
    }


    //private void Destination()
    //{
    //    if (_playerDetectedScript.IsPlayerVisible)
    //    {
    //        DetectionOfPlayer();
    //        if (_playerDetectedScript.Shadow != null)
    //        {
    //            _shadowPosition = _playerDetectedScript.Shadow.transform.position;
    //            _enemy.transform.LookAt(_shadowPosition);
    //        }
    //    }
    //    if (_isMovingToShadow)
    //    {
    //        if (_playerDetectedScript.Shadow != null)
    //        {
    //            if (Vector3.Distance(_enemy.transform.position, _playerDetectedScript.Shadow.transform.position) <= 2f)
    //            {
    //                Destroy(_playerDetectedScript.Shadow);
    //                _agent.ResetPath();
    //                _isMovingToShadow = false;
    //            }
    //        }
    //    }
    //}


    PatrolEnemy _patrolEnemyScript;
    PlayerDetected _playerDetectedScript;
    GameObject _enemy;
    NavMeshAgent _agent;
    GameObject _player;
    float timer = 0f;
    Vector3 _shadowPosition;
}
