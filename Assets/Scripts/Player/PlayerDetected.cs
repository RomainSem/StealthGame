using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetected : MonoBehaviour
{
    #region Exposed

    [SerializeField] GameObject _playerShadow;
    [SerializeField] Light _cameraLight;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {

    }

    #endregion

    #region Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CameraCone")
        {
            if (IsRaycastHittingPlayer)
            {
                IsDetected = true;
            }
            //_cameraLight.color = Color.red;
        }
        if (other.gameObject.tag == "EnemyCone")
        {
            IsDetected = true;
            //IsPlayerVisible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CameraCone")
        {
            if (Shadow != null)
            {
                Destroy(Shadow);
            }
            if (IsRaycastHittingPlayer)
            {
                Shadow = Instantiate(_playerShadow, transform.position, Quaternion.identity);
            }
            IsDetected = false;
            _cameraLight.color = Color.white;
        }
    }


    #endregion

    #region Private & Protected

    bool _isRaycastHittingPlayer;
    bool _isDetected;
    GameObject _shadow;

    public bool IsRaycastHittingPlayer { get => _isRaycastHittingPlayer; set => _isRaycastHittingPlayer = value; }
    public bool IsDetected { get => _isDetected; set => _isDetected = value; }
    public GameObject Shadow { get => _shadow; private set => _shadow = value; }
    public Light CameraLight { get => _cameraLight; set => _cameraLight = value; }

    #endregion
}
