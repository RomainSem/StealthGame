using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy = animator.gameObject;
        _patrolEnemyScript = _enemy.GetComponent<PatrolEnemy>();
        _playerDetectedScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDetected>();
        _agent = _enemy.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_patrolEnemyScript.BackAndForth)
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
        if (_playerDetectedScript != null)
        {
            animator.SetBool("IsPlayerVisible", _playerDetectedScript.IsPlayerVisible);
        }
    }

    private void GoToNextPoint()
    {
        if (_agent.remainingDistance <= 1)
        {
            _currentPoint++;
            if (_currentPoint >= _patrolEnemyScript.Waypoints.Length)
            {
                if (_patrolEnemyScript.BackAndForth)
                {
                    _isGoing = false;
                    _currentPoint = _patrolEnemyScript.Waypoints.Length - 1;
                }
                else
                {
                    _currentPoint = 0;
                }
            }
            _agent.SetDestination(_patrolEnemyScript.Waypoints[_currentPoint].position);
        }
    }

    private void GoToPreviousPoint()
    {
        if (_agent.remainingDistance <= 1)
        {
            _currentPoint--;

            if (_currentPoint < 0)
            {
                if (_patrolEnemyScript.BackAndForth)
                {
                    _isGoing = true;
                    _currentPoint = 0;
                }
                else
                {
                    _currentPoint = _patrolEnemyScript.Waypoints.Length - 1;
                }
            }
            _agent.SetDestination(_patrolEnemyScript.Waypoints[_currentPoint].position);
        }
    }

    NavMeshAgent _agent;
    int _currentPoint = 0;
    bool _isGoing;
    PatrolEnemy _patrolEnemyScript;
    PlayerDetected _playerDetectedScript;
    GameObject _enemy;
}