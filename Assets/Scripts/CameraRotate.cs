using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    #region Exposed

    [SerializeField] float _rotationSpeed = 0.5f;
    [SerializeField] Transform _leftPos;
    [SerializeField] Transform _rightPos;


    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
    }

    void Start()
    {
        _rotateTarget = _leftPos;
    }

    void Update()
    {
        CameraRotation();
    }

    private void FixedUpdate()
    {
        
    }

    #endregion

    #region Methods

    private void CameraRotation()
    {
        Vector3 lookPosition = Vector3.RotateTowards(transform.forward, _rotateTarget.localPosition, _rotationSpeed * Time.deltaTime, 0f);
        Quaternion lookRotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = lookRotation;
        Quaternion targetRotation = Quaternion.LookRotation(_rotateTarget.localPosition);
        if (Quaternion.Angle(transform.rotation, targetRotation) <= 0.5f)
        {
            _goLeft = !_goLeft;
            _rotateTarget = _goLeft ? _leftPos : _rightPos;
        }
    }

    #endregion

    #region Private & Protected

    Transform _rotateTarget;
    bool _goLeft;

    #endregion
}
