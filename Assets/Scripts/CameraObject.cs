using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera; 
    [SerializeField] private GameObject cameraModel; 
    [SerializeField] private bool autoRotate = true;
    [SerializeField] private float turnAmount = 20f;
    [SerializeField] private float turnSpeed = 0.4f;
    private float startYRotation;
    private bool activeCamera;
    private bool reverseTurn = false;

    private void Awake() 
    {
        if (autoRotate)
        {
            startYRotation = cameraModel.transform.rotation.eulerAngles.y; 
        }   
    }

    private void Update() 
    {
        if (!autoRotate) {return;}

        Vector3 targetRotation;
        if (!reverseTurn)
        {
            targetRotation = new Vector3(cameraModel.transform.eulerAngles.x, startYRotation + turnAmount, cameraModel.transform.eulerAngles.z);       
        }
        else
        {
            targetRotation = new Vector3(cameraModel.transform.eulerAngles.x, startYRotation - turnAmount, cameraModel.transform.eulerAngles.z);
        }
        cameraModel.transform.rotation = Quaternion.Lerp(cameraModel.transform.rotation, Quaternion.Euler(targetRotation), turnSpeed * Time.deltaTime);

        if ((cameraModel.transform.eulerAngles - targetRotation).magnitude < 5f)
        {
            reverseTurn = !reverseTurn;
        }
    }

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
