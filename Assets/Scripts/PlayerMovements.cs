using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//RequireComponent(typeof(Rigidbody));
public class PlayerMovements : MonoBehaviour
{
    #region Expose
    [Header("Movements")]
    [SerializeField] float _joggingSpeed = 7;
    [SerializeField] float _runningSpeed = 0.5f;
    [SerializeField] float _sneakingSpeed = 2;
    [SerializeField] float _rotationSpeed = 5;
    [SerializeField] float _jumpForce = 5;

    [Header("Floor Detection")]
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Vector3 _boxDimension;
    [SerializeField] Transform _groundChecker;
    [SerializeField] float _floorYOffeset = 1f;


    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {
        _rgdbody = GetComponent<Rigidbody>();
        _floorDetector = GetComponentInChildren<FloorDetector>();
    }

    void Start()
    {
        _mainCamera = Camera.main;
        _groundChecker = GameObject.Find("GroundChecker").transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Jump();
        Run();
        Sneak();

        Collider[] groundColliders = Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);
        _isGrounded = groundColliders.Length > 0;

    }

    private void FixedUpdate()
    {
        if (_isGrounded)
        {
            StickToGround();

            if (_isJumping)
            {
                _isGrounded = false;
                _direction.y = _jumpForce;
                _isJumping = false;
            }
        }
        else
        {
            //Ici soit on saute soit on tombe
            _direction.y = _rgdbody.velocity.y;
        }

        _rgdbody.velocity = _direction;
        RotateTowardsCamera();
    }


    #endregion

    #region Methods

    private void Move()
    {
        _direction = _mainCamera.transform.forward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
        _direction *= _joggingSpeed;
        _direction.y = 0; // Pour ne pas bouger en Y par rapport à la caméra
    }

    private void Run()
    {
        if (Input.GetButton("Fire3"))
        {
            _direction *= _runningSpeed;
        }
    }

    private void Sneak()
    {
        if (Input.GetButton("CTRL"))
        {
            if (_isSneaking == false)
            {
                _isSneaking = true;
                Debug.Log("Sneaking");
                _direction *= _sneakingSpeed;
            }
            else
            {
                _isSneaking = false;
            }

        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
        }
    }

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

    private void StickToGround()
    {
        Vector3 averagePosition = _floorDetector.AverageHeight();
        Vector3 newPosition = new Vector3(_rgdbody.position.x, averagePosition.y + _floorYOffeset, _rgdbody.position.z);

        //_rgdbody.MovePosition(newPosition);
        transform.position = newPosition;
        _direction.y = 0;
        //_rgdbody.position = new Vector3(_rgdbody.position.x, 0, _rgdbody.position.z) ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_groundChecker.position, _boxDimension * 2);
    }

    #endregion

    #region Private & Protected
    Camera _mainCamera;
    Rigidbody _rgdbody;
    FloorDetector _floorDetector;
    Vector3 _direction = new Vector3();
    bool _isJumping;
    public bool _isGrounded;
    bool _isSneaking;

    #endregion
}
