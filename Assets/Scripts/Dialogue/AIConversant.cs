using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Dialogue {
public class AIConversant : MonoBehaviour
{

    [SerializeField] Dialogue dialogue = null;

    private bool inTalkingDistance = false;
    PlayerConversant playerConversant;

    void Start() {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
    }
    private void FixedUpdate() {
        if (inTalkingDistance) {
            StartConversation();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            inTalkingDistance = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            inTalkingDistance = false;
        }
    }

    private void StartConversation() {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (dialogue == null) {return;}
            SetPlayerLookAt();
            playerConversant.StartDialogue(dialogue);

        }
    }

    private void SetPlayerLookAt() {
        PlayerController.Instance.playerLookAt = this.transform.position; 
    }

    
}
}
