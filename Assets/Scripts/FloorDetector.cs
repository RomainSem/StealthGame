using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    #region Expose

    [SerializeField] Transform[] _rayOrigins;
    [SerializeField] float _rayLength = 1.5f;
    [SerializeField] LayerMask _groundMask;
    
    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        
    }


    #endregion

    #region Methods

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform o in _rayOrigins)
        {
            Gizmos.DrawRay(o.position, Vector3.down * _rayLength);
        }
    }

    public Vector3 AverageHeight()
    {
        int hitCount = 0;
        Vector3 combinedPosition = Vector3.zero;

        RaycastHit hit;
        foreach (Transform o in _rayOrigins)
        {
            if (Physics.Raycast(o.position, Vector3.down, out hit, _rayLength, _groundMask))
            {
                hitCount++;
                // La position dans le world ou le raycast a touché le collider
                combinedPosition += hit.point;
            }
        }

        Vector3 averagePosition = Vector3.zero;
        if (hitCount > 0)
        {
            averagePosition = combinedPosition / hitCount;
        }
        return averagePosition;
    }

    #endregion

    #region Private & Protected

    #endregion
}
