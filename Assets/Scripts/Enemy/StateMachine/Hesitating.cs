using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Hesitating : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDetectedScript = _player.GetComponent<PlayerDetected>();
        _enemy = animator.gameObject;
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
                timer += Time.deltaTime;
                //if (timer >= 1.5f)
                //{
                //    _enemy.transform.LookAt(_player.transform.position);
                //    _playerDetectedScript.IsRaycastHittingPlayer = true;
                //}
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                _playerDetectedScript.IsEnemyRayHittingPlayer = false;
                timer = 0f;
            }
            if (_playerDetectedScript.IsDetectedByCam)
            {
                timer = 10f;
            }

            animator.SetBool("IsPlayerVisible", _playerDetectedScript.IsDetectedByEnemy);
            animator.SetFloat("TimerToStartPursuit", timer);
        }
    }


    PlayerDetected _playerDetectedScript;
    GameObject _enemy;
    GameObject _player;
    float timer = 0f;
}
