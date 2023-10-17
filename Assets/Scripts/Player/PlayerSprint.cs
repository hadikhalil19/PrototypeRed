using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprint : MonoBehaviour
{
    [SerializeField] TrailRenderer myTrailRenderer;
    private PlayerControls playerControls;
    private Animator myAnimator;
    [SerializeField] float sprintSpeed = 6f;
    private PlayerController playerController;
    private Rigidbody2D myRigidBody;


    private void Awake() {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        playerController = GetComponent<PlayerController>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }


    void Start()
    {
        //playerControls.Combat.Roll.performed += _ => Roll();
    }

    private void FixedUpdate() {
       
    }
    private void lockMovement() {
        playerController.MoveLock = true;
    }
    private void unlockMovement() {
        playerController.MoveLock = false;
    }

    private void lockAttack() {
        playerController.AttackLock = true;
    }
    private void unlockAttack() {
        playerController.AttackLock = false;
    }




}
