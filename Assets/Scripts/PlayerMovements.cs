using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//RequireComponent(typeof(Rigidbody));
public class PlayerMovements : MonoBehaviour
{
    #region Expose
    [Header("Movements")]
    [SerializeField] float _joggingSpeed = 5;
    [SerializeField] float _runningSpeed = 8;
    [SerializeField] float _sneakingSpeed = 3;
    [SerializeField] float _rotationSpeed = 5;
    [SerializeField] float _jumpForce = 5;

    [Header("Floor Detection")]
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Vector3 _boxDimension;

    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {
        _rgdbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _mainCamera = Camera.main;
        _groundChecker = GameObject.Find("GroundChecker").transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Jump();
        _direction = _mainCamera.transform.forward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
        _direction *= _joggingSpeed;
        Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);
    }

    private void FixedUpdate()
    {
        if (_isJumping)
        {
            _direction.y = _jumpForce;
            _isJumping = false;
        }
        else
        {
            // Fait une chute normale
            _direction.y = _rgdbody.velocity.y;
        }

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

            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            //Quaternion rotation = Quaternion.RotateTowards(_rgdbody.rotation, lookRotation, _rotationSpeed * Time.fixedDeltaTime);
            _rgdbody.MoveRotation(lookRotation);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
        }
    }

    #endregion

    #region Private & Protected
    Camera _mainCamera;
    Vector3 _direction = new Vector3();
    Rigidbody _rgdbody;
    bool _isJumping = false;
    Transform _groundChecker;

    #endregion
}
