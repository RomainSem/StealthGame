using UnityEngine;

//RequireComponent(typeof(Rigidbody));
public class PlayerMovements : MonoBehaviour
{
    #region Expose
    [Header("Movements")]
    [SerializeField] float _joggingSpeed = 7;
    [SerializeField] float _runningSpeed = 1.3f;
    [SerializeField] float _sneakingSpeed = 3f;
    [SerializeField] float _jumpForce = 6;
    [SerializeField] GameObject _3rdCamera;

    [Header("Floor Detection")]
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Vector3 _boxDimension;
    [SerializeField] Transform _groundChecker;
    [SerializeField] float _floorYOffeset = 1f;

    [Header("Slope handling")]
    [SerializeField] private float _maxSlopeAngle = 40f;


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
        DirectionFromInput();
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            IsJumping = true;
        }
        Run();
        Sneak();
        Speed();

        Collider[] groundColliders = Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);
        IsGrounded = groundColliders.Length > 0;

    }

    private void FixedUpdate()
    {
        if (IsGrounded)
        {
            StickToGround();
            if (_isJumping)
            {
                Jump();
            }
        }
        else
        {
            //Ici soit on saute soit on tombe
            _direction.y = _rgdbody.velocity.y;
        }
        if (SlopeAngle() > _maxSlopeAngle)
        {
            Debug.Log("Ne doit pas avancer");   // Mais quand même déjà un peu sur la pente
            Vector3 localDirection = transform.InverseTransformDirection(_direction);   //Passe du gloabal au local
            if (localDirection.z > 0) localDirection.z = 0;   //Pas le droit d'avancer
            Direction = transform.TransformDirection(localDirection);  //Repasse la direction en global
        }
        _rgdbody.velocity = _direction;
        RotateTowardsCamera();
    }


    #endregion

    #region Methods

    private void DirectionFromInput()
    {
        Direction = _mainCamera.transform.forward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
        Direction.Normalize();
        _direction.y = 0; // Pour ne pas bouger en Y par rapport à la caméra
    }

    private void Run()
    {
        if (Input.GetButton("Run"))
        {
            IsRunning = true;
            if (IsSneaking)
            {
                IsRunning = false;
            }
        }
        else
        {
            IsRunning = false;
        }
    }

    private void Sneak()
    {
        if (Input.GetButtonDown("Sneak"))
        {
            if (IsSneaking == false)
            {
                IsSneaking = true;
                Debug.Log("Sneaking");
                //Direction = _mainCamera.transform.forward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
                //Direction *= _sneakingSpeed;
            }
            else
            {
                IsSneaking = false;
            }

        }
    }

    private void Jump()
    {
        IsGrounded = false;
        _direction.y = _jumpForce;
        IsJumping = false;
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

    private float SlopeAngle()
    {
        Vector3 startingpoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
        Debug.DrawRay(startingpoint, Vector3.down);
        if (Physics.Raycast(startingpoint, Vector3.down, out _slopeHit))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle;
        }
        return 370;
    }

    private void Speed()
    {
        if (IsRunning)
        {
            Direction *= _runningSpeed;
        }
        if (IsSneaking)
        {
            Direction *= _sneakingSpeed;
        }
        else
        {
            Direction *= _joggingSpeed;
        }
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
    bool _isRunning;
    bool _isJumping;
    bool _isSneaking;
    private bool isGrounded;
    private RaycastHit _slopeHit;

    public bool IsGrounded { get => isGrounded; private set => isGrounded = value; }
    public Vector3 Direction { get => _direction; private set => _direction = value; }
    public bool IsJumping { get => _isJumping;  set => _isJumping = value; }
    public bool IsSneaking { get => _isSneaking; private set => _isSneaking = value; }
    public bool IsRunning { get => _isRunning; set => _isRunning = value; }

    #endregion
}
