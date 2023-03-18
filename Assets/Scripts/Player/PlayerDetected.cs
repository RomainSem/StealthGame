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
            if (!IsPlayerVisible)
            {
                IsDetectedByCamera = true;
            }
            else
            {
                _cameraLight.color = Color.red;
            }
        }
        if (other.gameObject.tag == "EnemyCone")
        {
            IsPlayerVisible = true;
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
            if (IsPlayerVisible)
            {
                Shadow = Instantiate(_playerShadow, transform.position, Quaternion.identity);
            }
            _cameraLight.color = Color.white;
        }
    }


    #endregion

    #region Private & Protected

    bool _isPlayerVisible;
    bool _isDetectedByCamera;
    GameObject _shadow;

    public bool IsPlayerVisible { get => _isPlayerVisible; set => _isPlayerVisible = value; }
    public bool IsDetectedByCamera { get => _isDetectedByCamera; set => _isDetectedByCamera = value; }
    public GameObject Shadow { get => _shadow; private set => _shadow = value; }

    #endregion
}
