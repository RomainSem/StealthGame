using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetected : MonoBehaviour
{
    #region Exposed

    [SerializeField] GameObject _playerShadow;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    #endregion

    #region Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CameraCone")
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
            Shadow = Instantiate(_playerShadow, transform.position, Quaternion.identity);
            // _patrolEnemy.SetInterest(Shadow);
            IsPlayerVisible = false;
        }
    }


    #endregion

    #region Private & Protected

    bool _isPlayerVisible;
    GameObject _shadow;

    public bool IsPlayerVisible { get => _isPlayerVisible; set => _isPlayerVisible = value; }
    public GameObject Shadow { get => _shadow; set => _shadow = value; }

    #endregion
}
