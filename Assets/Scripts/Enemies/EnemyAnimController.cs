using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{

    private bool hitStagger = false;
    public bool isAttacking = false;
    private Animator myAnimator;

    readonly int RESET_HASH = Animator.StringToHash("Reset");
    readonly int TAKEDAMAGE_HASH = Animator.StringToHash("TakeDamage");
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int ISATTACKING_HASH = Animator.StringToHash("isAttacking");
    readonly int STAGGER_HASH = Animator.StringToHash("Stagger");
    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }

    private void Update() {
        HitStaggerAnimEnd();
        AttackAnimEnd();
    }
    
    public void PlayHitAnim() {
        myAnimator.SetBool(STAGGER_HASH, true);
        myAnimator.SetTrigger(TAKEDAMAGE_HASH);
        myAnimator.SetBool(RESET_HASH, false);
        myAnimator.SetBool(ISATTACKING_HASH, false);
        hitStagger = true;
        isAttacking = false;
    }

    private void HitStaggerAnimEnd() {
        if (!hitStagger) {return;}
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("HitStagger")) 
        {
            myAnimator.SetBool(RESET_HASH, true);
            myAnimator.SetBool(STAGGER_HASH, false);
            
        }
    }

    public void PlayDeathAnim() {
        myAnimator.SetBool(DEATH_HASH, true);
    }

    public void PlayAttackAnim() {
        myAnimator.SetTrigger(ATTACK_HASH);
        myAnimator.SetBool(ISATTACKING_HASH, true);
        isAttacking = true;
    }

    private void AttackAnimEnd() {
        if (!isAttacking) {return;}
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) 
        {
            myAnimator.SetBool(ISATTACKING_HASH, false);
            isAttacking = false;
        }
    }
}
