using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.SceneManagement {
public class AreaEnterance : MonoBehaviour
{
   [SerializeField] private string transitionName;
   [SerializeField] private float autoSaveDelay = 0.2f;

    private void Start() {
        if (transitionName == SceneManagement.Instance.SceneTransitionName) {
            SceneManagement.Instance.EnableSaving();
            SceneManagement.Instance.EnableCameraController();

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Load();

            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();

            UIFade.Instance.FadeOut();

            StartCoroutine(checkpointSave());
        }
    }

    private IEnumerator checkpointSave() {
        yield return new WaitForSeconds(autoSaveDelay);
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save();
    }
}
}