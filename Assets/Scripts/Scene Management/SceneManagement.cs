using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.SceneManagement {
public class SceneManagement : Singleton<SceneManagement>
{
    [SerializeField] GameObject CameraController;
    [SerializeField] GameObject Saving;
    public string SceneTransitionName {get; private set;}

    public void SetTransitionName(string sceneTransitionName) {
        this.SceneTransitionName = sceneTransitionName;
    }
    
    public void EnableCameraController () {
        CameraController.SetActive(true);
    }

    public void EnableSaving() {
        Saving.SetActive(true);
    }

}
}
