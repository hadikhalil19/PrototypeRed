using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimHandler : MonoBehaviour
{
    private Animator myAnimator;
    private PlayerController playerController;

    public bool GetArrowRelease {get; set;}

    private void Awake() {
        myAnimator = GetComponentInParent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }
    private void ArrowReleaseEvent(){
        GetArrowRelease = true;
    }


    //public bool GetArrowRelease  { get { return arrowRelease;}}
}
