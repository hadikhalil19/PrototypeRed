using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.Saving;

namespace Proto.SceneManagement {
public class SavingWrapper : MonoBehaviour
{

    const string defaultSaveFile = "save";
    [SerializeField] float fadeInTime = 2f;

        IEnumerator Start() {
            UIFade.Instance.FadeIn();

            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            CameraController.Instance.SetPlayerCameraFollow();

            yield return new WaitForSeconds(fadeInTime);
            UIFade.Instance.FadeOut();
        }
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.L))
            {
                
                StartCoroutine(LoadFadeRoutine());
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Save();
            } 
    }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        private IEnumerator LoadFadeRoutine() {
        UIFade.Instance.FadeIn();
        yield return  new WaitForSeconds(fadeInTime);
        Load();
        yield return  new WaitForSeconds(fadeInTime);
        UIFade.Instance.FadeOut();
        }
}


}
