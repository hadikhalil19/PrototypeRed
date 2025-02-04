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
            UIFade.Instance.FadeIn(1);

            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            CameraController.Instance.SetPlayerCameraFollow();

            yield return new WaitForSeconds(fadeInTime);
            UIFade.Instance.FadeOut(1);
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
            Debug.Log("Game Saved");
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
            Debug.Log("Game Loaded");
        }

        private IEnumerator LoadFadeRoutine() {
        UIFade.Instance.FadeIn(1);
        yield return  new WaitForSeconds(fadeInTime);
        Load();
        yield return  new WaitForSeconds(fadeInTime);
        UIFade.Instance.FadeOut(1);
        }
}


}
