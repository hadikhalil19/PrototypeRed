using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proto.SceneManagement;
using Proto.SceneManagement.UI;

namespace Proto.SceneManagement {
public class StartMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 2f;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject QuestUI;
    [SerializeField] GameObject StartMenuWindow;

    GameObject dialogueUI;
    //[SerializeField] GameObject CameraController;



    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject.GetComponent<PlayerController>()) {
    //         SceneManagement.Instance.SetTransitionName(sceneTransitionName);
    //         UIFade.Instance.FadeIn();

    //         SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
    //         wrapper.Save();
            
    //         StartCoroutine(LoadSceneRoutine());
    //     }
    // }

    private void Start() {
        dialogueUI = transform.Find("Dialogue UI").gameObject;
        DisableDialogueUI();
    }

    public void EnableDialogueUI() {
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.GetComponent<DialogueUI>().LinkPlayerConversant();
    }

    public void DisableDialogueUI() {
        dialogueUI.gameObject.SetActive(false);
    }

    private IEnumerator LoadSceneRoutine() {
        yield return  new WaitForSeconds(waitToLoadTime);
        HUD.SetActive(true);
        QuestUI.SetActive(true);
        StartMenuWindow.SetActive(false);
        SceneManager.LoadScene(sceneToLoad);
        
    }
    
    public void OnPlayButton() {
        //SceneManager.LoadScene(1);
        Debug.Log("Start");
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        UIFade.Instance.FadeIn();
        StartCoroutine(LoadSceneRoutine());
        //CameraController.SetActive(true);
    }

    public void OnQuitButton () {
        Application.Quit();
    }
    

}
}
