using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.SceneManagement {
public class AreaEnterance : MonoBehaviour
{
   [SerializeField] private string transitionName;

    private void Start() {
        if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Load();

            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();

            wrapper.Save();

            UIFade.Instance.FadeOut();
        }
    }
}
}