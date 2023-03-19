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
        if (other.gameObject.tag == "CameraCone" && IsCamRayHittingPlayer)
        {
            IsDetected = true;
        }
        if (other.gameObject.tag == "EnemyCone")
        {
            if (IsEnemyRayHittingPlayer)
            {
                IsDetected = true;
            }
            else if (IsDetected && !IsEnemyRayHittingPlayer)
            {
                Shadow = Instantiate(_playerShadow, transform.position, Quaternion.identity);
                IsDetected = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CameraCone")
        {
            if (IsCamRayHittingPlayer)
            {
                if (Shadow != null)
                {
                    Destroy(Shadow);
                }
                Shadow = Instantiate(_playerShadow, transform.position, Quaternion.identity);
            }
            //IsDetected = false;
            _cameraLight.color = Color.white;
        }
        if (other.gameObject.tag == "Enemy")
        {
            IsDetected = false;
        }
    }


    #endregion

    #region Private & Protected

    bool _isCamRayHittingPlayer;
    bool _isEnemyRayHittingPlayer;
    bool _isDetected;
    GameObject _shadow;

    public bool IsCamRayHittingPlayer { get => _isCamRayHittingPlayer; set => _isCamRayHittingPlayer = value; }
    public bool IsEnemyRayHittingPlayer { get => _isEnemyRayHittingPlayer; set => _isEnemyRayHittingPlayer = value; }
    public bool IsDetected { get => _isDetected; set => _isDetected = value; }
    public GameObject Shadow { get => _shadow; private set => _shadow = value; }
    public Light CameraLight { get => _cameraLight; set => _cameraLight = value; }

    #endregion
}
