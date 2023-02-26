using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera; 
    [SerializeField] private bool autoRotate = true;
    [SerializeField] private float turnAmount = 20f;
    [SerializeField] private float turnSpeed = 0.4f;
    private float startYRotation;
    private bool activeCamera;
    private bool reverseTurn = false;

    private void Awake() 
    {
        startYRotation = virtualCamera.transform.rotation.eulerAngles.y;    
    }

    private void Update() 
    {
        if (!autoRotate) {return;}

        Vector3 targetRotation;
        if (!reverseTurn)
        {
            targetRotation = new Vector3(virtualCamera.transform.eulerAngles.x, startYRotation + turnAmount, virtualCamera.transform.eulerAngles.z);       
        }
        else
        {
            targetRotation = new Vector3(virtualCamera.transform.eulerAngles.x, startYRotation - turnAmount, virtualCamera.transform.eulerAngles.z);
        }
        virtualCamera.transform.rotation = Quaternion.Lerp(virtualCamera.transform.rotation, Quaternion.Euler(targetRotation), turnSpeed * Time.deltaTime);

        if ((virtualCamera.transform.eulerAngles - targetRotation).magnitude < 5f)
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
