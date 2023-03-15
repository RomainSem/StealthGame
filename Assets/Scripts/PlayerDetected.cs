using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetected : MonoBehaviour
{
    #region Exposed

    

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
            IsPlayerVisible = false;
        }
    }

    #endregion

    #region Private & Protected

    bool _isPlayerVisible;

    public bool IsPlayerVisible { get => _isPlayerVisible; set => _isPlayerVisible = value; }

    #endregion
}
