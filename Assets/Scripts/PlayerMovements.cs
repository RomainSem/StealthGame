using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovements : MonoBehaviour
{
    #region Expose
    [SerializeField] float _joggingSpeed = 2;
    [SerializeField] float _runningSpeed = 3;
    [SerializeField] float _sneakingSpeed = 1;
    
    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {
        _mainCamera = Camera.main;
        _rgdbody = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }


    #endregion

    #region Methods

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // Calculer la direction globale en fonction de la direction de la caméra
        Vector3 cameraForward = _mainCamera.transform.TransformDirection(Vector3.forward);
        cameraForward.y = 0f;
        Vector3 cameraRight = _mainCamera.transform.TransformDirection(Vector3.right);
        cameraRight.y = 0f;
        _direction = cameraForward * vertical + cameraRight * horizontal;
        _direction.Normalize();

        _rgdbody.AddForce(_direction * _joggingSpeed, ForceMode.Force);
    }

    #endregion

    #region Private & Protected
    Camera _mainCamera;
    Vector3 _direction;
    Rigidbody _rgdbody;

    #endregion
}
