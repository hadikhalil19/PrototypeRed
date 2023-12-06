using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    private Animator myAnimator;
    private PlayerControls playerControls;
    [SerializeField] private GameObject interactIcon;

    private bool inRange = false;
    bool chestOpen = false;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        playerControls.Interact.Interact.performed += _ => InteractWithObject();
    }


    public void SwitchState(bool chestOpen) {
        if (chestOpen) {
            myAnimator.Play("ChestOpen");
            PickUpSpawner pickUpSpawner = GetComponent<PickUpSpawner>();
            pickUpSpawner?.DropItems();
        } else {
            myAnimator.Play("ChestClosed");
        }
    }

    private void InteractWithObject() {
        if (inRange && !chestOpen) {
            chestOpen = true;
            SwitchState(chestOpen);
            interactIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && !chestOpen)
        {
            inRange = true;
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && !chestOpen)
        {
            inRange = false;
            interactIcon.SetActive(false);
        }
    }

}
