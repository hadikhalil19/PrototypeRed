using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Dialogue {
public class AIConversant : MonoBehaviour
{

    [SerializeField] Dialogue dialogue = null;
    [SerializeField] string conversantName;

    [SerializeField] private GameObject interactIcon;

    private bool inTalkingDistance = false;
    PlayerConversant playerConversant;
    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    void Start() {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerControls.Interact.Interact.performed += _ => StartConversation();
    }
   
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            inTalkingDistance = true;
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            inTalkingDistance = false;
            interactIcon.SetActive(false);
        }
    }


    private void StartConversation() {
        if (inTalkingDistance) {
            if (dialogue == null) {return;}
            SetPlayerLookAt();
            playerConversant.StartDialogue(this, dialogue);
        }
    }

    public string GetName()
        {
            return conversantName;
        }

    private void SetPlayerLookAt() {
        PlayerController.Instance.playerLookAt = this.transform.position; 
    }

    
}
}
