using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera; 
    private bool activeCamera;

    public void EnableCamera(bool enable)
    {
        virtualCamera.SetActive(enable);
        activeCamera = enable;
    }

    public bool IsCameraActive()
    {
        return activeCamera;
    }
}