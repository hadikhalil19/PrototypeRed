using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Proto.SceneManagement {

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] GameObject blockExitObject = null;

    private float waitToLoadTime = 1f;
   

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeIn(1);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();
            
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine() {
        yield return  new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoad);
        
    }

    public void SetBlockExit(bool blockExit) {
        blockExitObject.SetActive(blockExit);
    }
}

}

