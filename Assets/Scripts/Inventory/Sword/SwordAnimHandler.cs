using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimHandler : MonoBehaviour
{
   
    private Animator myAnimator;
    private PlayerController playerController;

    private bool slashHitbox = false;
    private bool stabHitbox = false;

    private void Awake() {
        myAnimator = GetComponentInParent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }
    private void SlashHitboxStartEvent(){
        slashHitbox = true;
        playerController.AttackMoving = true;
        playerController.AttackDirectionLock = true;
    }

    private void SlashHitboxEndEvent(){
        slashHitbox = false;
        playerController.AttackDirectionLock = false;
        
    }

    private void StabHitboxStartEvent(){
        stabHitbox = true;
        
    }

    private void StabHitboxEndEvent(){
        stabHitbox = false;
    }

    public bool GetSlashHitbox  { get { return slashHitbox;}}
    public bool GetStabHitbox {get {return stabHitbox;}}

    public void ResetSwordAnim() {
        stabHitbox = false;
        slashHitbox = false;
    }

}
