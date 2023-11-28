using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proto.SceneManagement;
using Proto.SceneManagement.UI;

namespace Proto.SceneManagement {
public class PauseMenu : MonoBehaviour
{
     [SerializeField] GameObject uiContainer = null;

    public void OnResumeButton() {
        uiContainer.SetActive(!uiContainer.activeSelf);
    }

    public void OnQuitButton () {
        Application.Quit();
    }
    
}
}
