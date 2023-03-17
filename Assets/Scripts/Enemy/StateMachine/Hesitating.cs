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
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerDetectedScript.IsDetectedByCamera)
        {
            _enemy.transform.LookAt(_player.transform.position);
            _playerDetectedScript.IsPlayerVisible = true;
            timer = 10f;
        }

        RaycastHit hit;
        if (Physics.Raycast(_enemy.transform.position, _player.transform.position - _enemy.transform.position, out hit))
        {

            if (hit.collider.gameObject.tag == "Player")
            {
                timer += Time.deltaTime;
                if (timer >= 1.5f)
                {
                    _enemy.transform.LookAt(_player.transform.position);
                    _playerDetectedScript.IsPlayerVisible = true;
                }
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                timer = 0f;
                _playerDetectedScript.IsPlayerVisible = false;
                animator.SetBool("IsPlayerVisible", _playerDetectedScript.IsPlayerVisible);
            }

            animator.SetFloat("TimerToStartPursuit", timer);
        }
    }


    PlayerDetected _playerDetectedScript;
    GameObject _enemy;
    GameObject _player;
    float timer = 0f;
}
