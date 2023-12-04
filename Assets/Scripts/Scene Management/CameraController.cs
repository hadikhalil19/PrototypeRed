using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    //private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineStateDrivenCamera cinemachineStateDrivenCamera;
    private CinemachineController cinemachineController;
    private bool defaultCamera = true;
    
    private void Start() {
        SetPlayerCameraFollow();
    }
    

    public void SetPlayerCameraFollow() {
        
        //cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        //cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;

        cinemachineStateDrivenCamera = FindObjectOfType<CinemachineStateDrivenCamera>();
        cinemachineStateDrivenCamera.Follow = PlayerController.Instance.transform;
    }

    public void SwitchToIndoorCamera() {
        cinemachineController = FindObjectOfType<CinemachineController>();
        defaultCamera = false;
        cinemachineController.SwitchState(defaultCamera);
        
    }

    public void SwitchToDefaultCamera() {
        cinemachineController = FindObjectOfType<CinemachineController>();
        defaultCamera = true;
        cinemachineController.SwitchState(defaultCamera);
        
    }

}


