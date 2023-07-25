using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffectControl : MonoBehaviour
{
    private Animator myAnimator;

    private bool clockwise = true;

    private SwordAttack swordAttack;
    private void Awake() {
        myAnimator = GetComponentInParent<Animator>();
        swordAttack = FindObjectOfType<SwordAttack>();
    }

    private void FixedUpdate() {
        clockwise = swordAttack.Clockwise;
        myAnimator.SetBool("Clockwise", clockwise);
    }

}
