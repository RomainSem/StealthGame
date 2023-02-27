using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//RequireComponent(typeof(Rigidbody));
public class PlayerMovements : MonoBehaviour
{
    #region Expose
    [SerializeField] float _joggingSpeed = 5;
    [SerializeField] float _runningSpeed = 8;
    [SerializeField] float _sneakingSpeed = 3;
    [SerializeField] float _rotationSpeed = 3;

    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {
        _rgdbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        _direction = _mainCamera.transform.forward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
        _direction *= _joggingSpeed;
    }

    private void FixedUpdate()
    {
        // Fait une chute normale
        _direction.y = _rgdbody.velocity.y;

        RotateTowardsCamera();
        _rgdbody.velocity = _direction;
    }


    #endregion

    #region Methods

    private void RotateTowardsCamera()
    {
        if (_direction.magnitude > 0.1f)
        {
            Vector3 lookDirection = _mainCamera.transform.forward;
            lookDirection.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            _rgdbody.MoveRotation(rotation);
        }
    }

    #endregion

    #region Private & Protected
    Camera _mainCamera;
    Vector3 _direction = new Vector3();
    Rigidbody _rgdbody;

    #endregion
}
