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

    private void Update()
    {
        if (IsDetectedByCam)
        {
            IsDetectedByEnemy = true;
        }
        if (!IsDetectedByCam)
        {
            _cameraLight.color = Color.white;
        }
    }

    #endregion

    #region Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CameraCone" && IsCamRayHittingPlayer)
        {
            IsDetectedByCam = true;
        }
        if (other.gameObject.tag == "EnemyCone")
        {
            if (IsEnemyRayHittingPlayer)
            {
                IsDetectedByEnemy = true;
            }
            else if (IsDetectedByCam && !IsEnemyRayHittingPlayer)
            {
                Shadow = Instantiate(_playerShadow, transform.position, Quaternion.identity);
                IsDetectedByEnemy = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CameraCone")
        {
            IsDetectedByCam = false;
            if (IsCamRayHittingPlayer)
            {
                if (Shadow != null)
                {
                    Destroy(Shadow);
                }
                Shadow = Instantiate(_playerShadow, transform.position, Quaternion.identity);
            }
            _cameraLight.color = Color.white;
        }
        if (other.gameObject.tag == "Enemy")
        {
            IsDetectedByEnemy = false;
        }
    }


    #endregion

    #region Private & Protected

    bool _isCamRayHittingPlayer;
    bool _isEnemyRayHittingPlayer;
    bool _isDetectedByCam;
    bool _isDetectedByEnemy;
    GameObject _shadow;

    public bool IsCamRayHittingPlayer { get => _isCamRayHittingPlayer; set => _isCamRayHittingPlayer = value; }
    public bool IsEnemyRayHittingPlayer { get => _isEnemyRayHittingPlayer; set => _isEnemyRayHittingPlayer = value; }
    public bool IsDetectedByCam { get => _isDetectedByCam; set => _isDetectedByCam = value; }
    public bool IsDetectedByEnemy { get => _isDetectedByEnemy; set => _isDetectedByEnemy = value; }
    public GameObject Shadow { get => _shadow; private set => _shadow = value; }
    public Light CameraLight { get => _cameraLight; set => _cameraLight = value; }

    #endregion
}
