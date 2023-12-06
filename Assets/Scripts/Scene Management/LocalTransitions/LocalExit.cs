using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.SceneManagement {
public class LocalExit : MonoBehaviour
{
    [SerializeField] private Transform moveToLocation;
    [SerializeField] private float fadeDelay = 1f;
    [SerializeField] private float fadeSpeedMult = 2f;
    [SerializeField] private bool moveIndoor = false;
   

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            UIFade.Instance.FadeIn(fadeSpeedMult);
            StartCoroutine(moveToRoutine());
        }
    }

    private IEnumerator moveToRoutine() {
        yield return  new WaitForSeconds(fadeDelay);
        PlayerController.Instance.gameObject.transform.position =  moveToLocation.position;
        if (moveIndoor) {
            CameraController.Instance.SwitchToIndoorCamera();
        } else {
            CameraController.Instance.SwitchToDefaultCamera();
        }
        
        StartCoroutine(fadeOutWaitRoutine());
    }

    private IEnumerator fadeOutWaitRoutine() {
        yield return  new WaitForSeconds(fadeDelay);
        UIFade.Instance.FadeOut(fadeSpeedMult);

    }

}
}
