using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETransparentDetection : MonoBehaviour
{
    private TransparentDetection transparentDetection;
    private void Awake() {
        transparentDetection = GetComponent<TransparentDetection>();
    }

    private void Update() {
        if (PlayerController.Instance.transform.position.y > this.transform.position.y) {
            transparentDetection.FadeStart();
        } else {
            transparentDetection.FadeEnd();
        }
        
    }
}
