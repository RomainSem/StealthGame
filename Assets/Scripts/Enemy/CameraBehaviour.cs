using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraBehaviour : MonoBehaviour
{
    #region Exposed

    [SerializeField] GameObject _player;
    [SerializeField] GameObject _playerShadow;
    [SerializeField] float _rotationSpeed = 0.5f;
    [SerializeField] Transform _leftPos;
    [SerializeField] Transform _rightPos;


    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        _playerDetectedScript = _player.GetComponent<PlayerDetected>();
    }

    void Start()
    {
        _rotateTarget = _leftPos;
    }

    void Update()
    {
        CameraRotation();
        CameraRaycast();
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

    private void CameraRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _player.transform.position - transform.position, out hit))
        {
            if (hit.collider.gameObject.tag == "Player"/* && _playerDetectedScript.IsDetected*/)
            {
                _playerDetectedScript.IsCamRayHittingPlayer = true;
                if (_playerDetectedScript.IsDetectedByCam)
                {
                    _playerDetectedScript.CameraLight.color = Color.red;
                }
                else if (_playerDetectedScript.IsDetectedByCam)
                {

                }
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                _playerDetectedScript.CameraLight.color = Color.white;
                _playerDetectedScript.IsCamRayHittingPlayer = false;
                if (!_playerDetectedScript.IsDetectedByCam)
                {
                    _playerDetectedScript.CameraLight.color = Color.white;
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        Gizmos.DrawLine(transform.position, transform.position + directionToPlayer);
    }

    #endregion

    #region Private & Protected

    Transform _rotateTarget;
    PlayerDetected _playerDetectedScript;
    bool _goLeft;

    #endregion
}
