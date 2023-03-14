using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    #region Exposed

    [SerializeField] Transform _target;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                _agent.SetDestination(hitInfo.point);

            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    #endregion

    #region Methods

    #endregion

    #region Private & Protected

    NavMeshAgent _agent;

    #endregion
}
