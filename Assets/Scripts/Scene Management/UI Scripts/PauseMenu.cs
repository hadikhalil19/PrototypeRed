//Pause the game and control the pause menu and allow the player to resume or quit the game.

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
    
    private void Update() {
        if (uiContainer.activeSelf) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }
}
}
