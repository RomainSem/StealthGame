using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region Exposed

    [SerializeField] Animator _animator;
    [SerializeField] PlayerMovements _controller;


    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _currentSpeed = _rb.velocity.magnitude;
        AnimationToPlay();
    }

    private void FixedUpdate()
    {
        
    }

    #endregion

    #region Methods

    private void AnimationToPlay()
    {
        //if (_controller.IsJumping == true)
        //{
            _animator.SetBool("isJumping", _controller.IsJumping);
            //_animator.SetBool("isGrounded", false);
        //}
        //if (_controller.IsGrounded == true)
        //{
            //_animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", _controller.IsGrounded);
            //_isFalling = false;
        //}
        //if (_currentSpeed > 0.3f)
        //{
            _localDirection = transform.InverseTransformDirection(_controller.Direction);   //Passe du gloabal au local
            _animator.SetFloat("moveSpeed", _currentSpeed);
            _animator.SetFloat("speedX", _localDirection.x);
            _animator.SetFloat("speedY", _localDirection.z);
            _animator.SetBool("isSneaking", _controller.IsSneaking);
        //}
        if (_currentSpeed < 0.3f)
        {
            _currentSpeed = 0f;
            _localDirection.y = 0;
            _localDirection.x = 0;
        }
        //if (_controller.IsGrounded == false)
        //{
        //    _isFalling = true;
        //}
        //if (_isFalling == true)
        //{
        //    _animator.SetBool("isJumping", false);
        //    _animator.SetBool("isGrounded", false);
        //}
    }

    #endregion

    #region Private & Protected

    private float _currentSpeed;
    private Rigidbody _rb;
    //private bool _isFalling;
    Vector3 _localDirection = new Vector3();

    #endregion
}
