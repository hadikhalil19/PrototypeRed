using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{

    private bool hitStagger = false;
    private bool takeMoveSample = true;
    private float moveSampleTime = 0.1f;
    public bool isAttacking = false;
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    readonly int RESET_HASH = Animator.StringToHash("Reset");
    readonly int TAKEDAMAGE_HASH = Animator.StringToHash("TakeDamage");
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int ISATTACKING_HASH = Animator.StringToHash("isAttacking");
    readonly int STAGGER_HASH = Animator.StringToHash("Stagger");
    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        //if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death")) 
        myAnimator.SetTrigger(DEATH_HASH);
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

    public void SetAnimMoveDirection(Vector2 movement) {
        if (takeMoveSample) {
            takeMoveSample = false;
            StartCoroutine(SampleMoveDirRoutine(movement)); // we do this minimize mili-sec anim direction changes to make it look smoother
        }
        
    }

    private IEnumerator SampleMoveDirRoutine(Vector2 movement) {
        yield return new WaitForSeconds(moveSampleTime);

        if (movement.magnitude > 0.1f) // if moving
        {
            if (movement.x < 0) {
                spriteRenderer.flipX = true;
            } else if (movement.x > 0){
                spriteRenderer.flipX = false;
            }
        }

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
        myAnimator.SetFloat("speed", movement.sqrMagnitude);
        takeMoveSample = true;
    }
}
