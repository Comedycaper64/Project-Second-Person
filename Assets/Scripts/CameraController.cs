using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputReader inputReader;
    private List<CameraObject> cameraObjects = new List<CameraObject>();
    [SerializeField] private CameraObject startCamera;
    [SerializeField] private CameraObject enabledCamera;
    private int raycastLayermask;
    [SerializeField] private AudioClip cameraSwitchSFX;

    private void Start() 
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("Camera");  
        foreach(GameObject camera in cameras)
        {
            cameraObjects.Add(camera.GetComponent<CameraObject>());
        }

        int layermask1 = 1 << 6;
        int layermask2 = 1 << 7;
        raycastLayermask = layermask1 | layermask2;

        inputReader = GameObject.FindGameObjectWithTag("Player").GetComponent<InputReader>();
        inputReader.SwitchCameraEvent += OnSwitchCamera;
        SwitchOffAllCameras();
        EnableStartCamera();
    }

    private void OnDestroy() 
    {
        inputReader.SwitchCameraEvent -= OnSwitchCamera;
    }

    private void EnableStartCamera()
    {
        enabledCamera = startCamera;
        enabledCamera.EnableCamera(true);
    }

    private void OnSwitchCamera(object sender, Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, raycastLayermask))
        {
            if (hit.collider.gameObject.TryGetComponent<CameraObject>(out CameraObject cameraObject))
            {
                enabledCamera.EnableCamera(false);
                enabledCamera = cameraObject;
                enabledCamera.EnableCamera(true);
                if (SoundManager.Instance)
                {
                    AudioSource.PlayClipAtPoint(cameraSwitchSFX, enabledCamera.transform.position, SoundManager.Instance.GetSoundEffectVolume());
                }
            }
        }
    }

    private void SwitchOffAllCameras()
    {
        foreach(CameraObject camera in cameraObjects)
        {
            camera.EnableCamera(false);
        }
    }
}
