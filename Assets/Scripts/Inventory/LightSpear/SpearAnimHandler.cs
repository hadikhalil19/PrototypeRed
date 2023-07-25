using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAnimHandler : MonoBehaviour
{
    private Animator myAnimator;
    private PlayerController playerController;

    //private bool throwing = false;
    public bool GetSpearRelease {get; set;}

    public bool GetThrowing  { get; private set;}

    private void Awake() {
        myAnimator = GetComponentInParent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }
    private void ThrowingStartEvent(){
        GetThrowing = true;
        playerController.AttackMoving = true;
        playerController.AttackDirectionLock = true;
        playerController.SpriteFlipLock = true;
    }

    private void ThrowingEndEvent(){
        GetThrowing = false;
        playerController.AttackDirectionLock = false;
        playerController.SpriteFlipLock = false;
    }

    private void SpearReleaseEvent(){
        GetSpearRelease = true;
    }


    
}
