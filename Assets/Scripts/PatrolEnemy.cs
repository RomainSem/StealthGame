using System.Collections;
using System.Collections.Generic;
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
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _agent.SetDestination(_waypoints[_currentPoint].position);
    }

    void Update()
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

    private void FixedUpdate()
    {

    }

    #endregion

    #region Methods

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
    bool _isGoing = false;

    #endregion
}
