using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.SceneManagement {
public class LocalExit : MonoBehaviour
{
    [SerializeField] private string moveToLocation;
    [SerializeField] private string localTransitionName;

    private float waitToLoadTime = 1f;
   

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            SceneManagement.Instance.SetTransitionName(localTransitionName);
            UIFade.Instance.FadeIn();

            
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine() {
        yield return  new WaitForSeconds(waitToLoadTime);
        // jump to location
        
    }


}
}
