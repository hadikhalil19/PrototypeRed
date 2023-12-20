// logic for player dodge rolls. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.Audio;

public class PlayerRoll : MonoBehaviour
{
    [SerializeField] TrailRenderer myTrailRenderer;
    private PlayerControls playerControls;
    private Animator myAnimator;
    bool isRolling = false;

    bool canRoll = true;
    Vector2 rollMoveDirection;
    [SerializeField] float rollSpeed = 2f;
    private PlayerController playerController;
    private Rigidbody2D myRigidBody;
    private FootstepAudioPlayer footstepAudioPlayer;


    private void Awake() {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        playerController = GetComponent<PlayerController>();
        myRigidBody = GetComponent<Rigidbody2D>();
        footstepAudioPlayer = GetComponentInChildren<FootstepAudioPlayer>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }


    void Start()
    {
        playerControls.Combat.Roll.performed += _ => Roll();
    }

    private void FixedUpdate() {
        if (isRolling) {
            RollMove();
        }
        RollAnimEnd();
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



// method for player dodge rolls. Called from player controls input. Sets roll direction and triggers roll animation. Locks player movement and attack.
    private void Roll() {
        if  (PlayerHealth.Instance.IsDead) { return; }
        if (playerController.movement.magnitude < 0.1f) {return;}
        if (playerController.MoveLock == false && canRoll) {
            myTrailRenderer.emitting = true;
            isRolling = true;
            canRoll = false;
            myAnimator.SetTrigger("Roll");
            myAnimator.SetBool("isRolling", true);
            rollMoveDirection = playerController.movement;
            myAnimator.SetFloat("rollX", rollMoveDirection.x);
            myAnimator.SetFloat("rollY", rollMoveDirection.y);
            footstepAudioPlayer.PlayRollAudio();
            lockAttack();
            lockMovement();
            StartCoroutine(RollCooldownRoutine());
        }
    }

// method for cooldown between rolls.
    private IEnumerator RollCooldownRoutine(){
        float rollCooldown = 1f;
        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
    }

// method for ending roll animation. Unlocks player movement and attack. Stops trail renderer.
    private void RollAnimEnd() {
        if (!isRolling) {return;}
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rolling")) 
        {
            isRolling = false;
            myAnimator.SetBool("isRolling", false);
            unlockAttack();
            unlockMovement();
            myTrailRenderer.emitting = false;
                    
        } else if (PlayerHealth.Instance.IsDead) {
            isRolling = false;
            myAnimator.SetBool("isRolling", false);
            unlockAttack();
            unlockMovement();
            myTrailRenderer.emitting = false; 
        }
    }

// rigidbody movement for roll.
     private void RollMove() {
        myRigidBody.MovePosition(myRigidBody.position + (rollMoveDirection.normalized * rollSpeed * Time.fixedDeltaTime));
        
    }

    private void RollIframeStartEvent () {
        PlayerHealth.Instance.rollInvulnerable = true;

    }

    private void RollIframeEndEvent () {
        PlayerHealth.Instance.rollInvulnerable = false;
    }


}
