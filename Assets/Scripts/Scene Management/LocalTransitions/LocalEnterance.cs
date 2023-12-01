using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.SceneManagement {

public class LocalEnterance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    [SerializeField] private float autoSaveDelay = 0.2f;

    private void Start() {
        PlayerController.Instance.transform.position = this.transform.position;
    }

    private IEnumerator checkpointSave() {
        yield return new WaitForSeconds(autoSaveDelay);
       
    }


}
}
