using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.Universal;

public class SwordAttack : MonoBehaviour, IWeapon
{
    
    private Animator myAnimator;
    [SerializeField] int attackCounter = 0;

    [SerializeField] int numberOfAttacks = 2;
    private bool isAttacking = false;
    public bool Clockwise  {get; private set;}
    private bool hasSlashEffect = false;

    private bool secondaryAttack = false;

    private bool shieldAction = false;

    [SerializeField] private Transform SlashCollider;
    [SerializeField] private Transform StabCollider;

    [SerializeField] private AudioSource stabAttackAudio; 
    private SwordAnimHandler swordAnimHandler;
    
    [SerializeField] private GameObject slashEffectPrefab;

    [SerializeField] private Transform slashEffectSpawn;

    private GameObject slashEffectAnim;
    [SerializeField] private WeaponInfo weaponInfo;
    private Vector2 stabMoveDirection;

    private bool delayedSecondaryStop;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int SECONDARY_HASH = Animator.StringToHash("Secondary");
    readonly int SHIELDUP_HASH = Animator.StringToHash("ShieldUp");
    readonly int ATTACKCounter_HASH = Animator.StringToHash("AttackCounter");
    readonly int ISATTACKING_HASH = Animator.StringToHash("isAttacking");
    //readonly int SWORDA1_HASH = Animator.StringToHash("SwordA1");
    readonly int SWORDA2_HASH = Animator.StringToHash("SwordA2");
    readonly int SWORDA3_HASH = Animator.StringToHash("SwordA3");
    readonly int SPRINTING_HASH = Animator.StringToHash("Sprinting");

    private void Awake() {
        myAnimator = PlayerController.Instance.GetComponent<Animator>();
        swordAnimHandler = FindObjectOfType<SwordAnimHandler>();
    }

    

    private void FixedUpdate() {
        AttackAnimEnd();
        toggleSlashHitbox();
        toggleStabHitbox();
        PlayerShieldAnim();
        MouseFollowWithOffset();
    }

    public void Attack() {
        if (PlayerController.Instance.AttackLock) {return;}
        if (PlayerMana.Instance.CurrentMana < weaponInfo.weaponManaCost) {return;}
        if (PlayerController.Instance.sprint) {
            RunStabAttack();
            return;
        }
        myAnimator.SetBool(SHIELDUP_HASH, false);
        shieldAction = false;
        if (!isAttacking) {
        attackCounter = 1;
        myAnimator.SetTrigger(ATTACK_HASH);
        myAnimator.SetInteger(ATTACKCounter_HASH, attackCounter);
        myAnimator.SetBool(ISATTACKING_HASH, true);
        Clockwise = true;
        isAttacking = true;
        lockMovement();
        } else if (isAttacking && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA2") ) {
        attackCounter = 2;
        myAnimator.SetInteger(ATTACKCounter_HASH, attackCounter);
        } else if (isAttacking && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA3") ) {
        attackCounter = 1;
        myAnimator.SetInteger(ATTACKCounter_HASH, attackCounter);
        } 
        
    }

    private void RunStabAttack() {
        myAnimator.SetTrigger(ATTACK_HASH);
        //myAnimator.SetBool(ISATTACKING_HASH, true);
        PlayerController.Instance.AttackMoving = true;
        PlayerController.Instance.sprintAttack = true;
        PlayerController.Instance.sprint = false;
        PlayerController.Instance.AttackLock = true;
        stabMoveDirection = PlayerController.Instance.movement;
        StabColliderFollow();
        myAnimator.SetFloat("rollX", stabMoveDirection.x);
        myAnimator.SetFloat("rollY", stabMoveDirection.y);
        lockMovement();
        //PlayerController.Instance.AttackDirectionLock = true;
        StabAttackEffects();
        StartCoroutine(StabEndRoutine());
    }

    private void StabAttackEffects() {
        stabAttackAudio.Play();
    }

    private IEnumerator StabEndRoutine() {
        yield return new WaitForSeconds(0.8f);
        unlockMovement();
        PlayerController.Instance.AttackMoving = false;
        PlayerController.Instance.sprintAttack = false;
        PlayerController.Instance.AttackLock = false;
        myAnimator.SetBool(SPRINTING_HASH, false);
    }

    public void SecondaryAttackStart() {
        if (PlayerController.Instance.AttackLock) {return;}
        secondaryAttack = true;
    }

    public void SecondaryAttackStop() {
        if (isAttacking) {
            secondaryAttack = false; 
            delayedSecondaryStop = true;
            return;
        }
        secondaryAttack = false;
        shieldAction = false;
        myAnimator.SetBool(SHIELDUP_HASH, false);
        PlayerHealth.Instance.shieldActive = false;
        unlockMovement();
    }

    private void DelaySecondaryStop() {
        shieldAction = false;
        myAnimator.SetBool(SHIELDUP_HASH, false);
        PlayerHealth.Instance.shieldActive = false;
        unlockMovement();
    }

    private void PlayerShieldAnim() {
        if (isAttacking) { return;}
        if (secondaryAttack && !shieldAction) {
            shieldAction = true;
            myAnimator.SetTrigger(SECONDARY_HASH);
            myAnimator.SetBool(SHIELDUP_HASH, true);
            lockMovement();
        } else if (shieldAction) {
            ShieldBlock();
        }
    }

    private void ShieldBlock() {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShieldIdle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShieldHit")) {
            PlayerHealth.Instance.shieldActive = true;
            PlayerHealth.Instance.shieldManaCost = weaponInfo.weaponManaCost;
        }
        else {
            PlayerHealth.Instance.shieldActive = false;
            PlayerHealth.Instance.shieldManaCost = 0;
        }
    }

    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }
    private void slashEffectTrigger() {
        slashEffectAnim = Instantiate(slashEffectPrefab, slashEffectSpawn.position, Quaternion.identity); // Slash effect
        slashEffectAnim.transform.parent = this.transform.parent; // Slash effect
        SlashEffectFollowMouse(); // Slash effect   
        PlayerMana.Instance.UseMana(weaponInfo.weaponManaCost);
    }

    private void toggleSlashHitbox(){
        if (swordAnimHandler.GetSlashHitbox) {
            if (!hasSlashEffect) {
                slashEffectTrigger();
                hasSlashEffect = true;
            }
            SlashCollider.gameObject.SetActive(true);
        } else {
            SlashCollider.gameObject.SetActive(false);
            hasSlashEffect = false;
        }
    }
    private void toggleStabHitbox(){
        if (!PlayerController.Instance.sprintAttack) {return;}
        if (swordAnimHandler.GetStabHitbox) {
            StabCollider.gameObject.SetActive(true);
        } else {
            StabCollider.gameObject.SetActive(false);
        }
    }

    private void AttackAnimEnd() {
        if (!isAttacking) {return;}
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA2") && myAnimator.GetBool(SWORDA2_HASH)) 
        {
            myAnimator.SetBool(SWORDA2_HASH, false);
            

        } else if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA3") && myAnimator.GetBool(SWORDA3_HASH)) 
        {
            myAnimator.SetBool(SWORDA3_HASH, false);
            Clockwise = !Clockwise; // Reset Clockwise attack effects

        }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA2")) 
        {
            isAttacking = false;
            unlockMovement();
            myAnimator.SetBool(ISATTACKING_HASH, false);
            resetCurrentAttackCounter();
            if (delayedSecondaryStop && !secondaryAttack) {DelaySecondaryStop();}         
        } else if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA3")) 
        {
            isAttacking = false;
            unlockMovement();
            myAnimator.SetBool(ISATTACKING_HASH, false);
            resetCurrentAttackCounter();
            if (delayedSecondaryStop && !secondaryAttack) {DelaySecondaryStop();} 
        }

    }

    private void resetCurrentAttackCounter() {
        if (attackCounter > numberOfAttacks) {
            attackCounter = 0;
        } 
    }

    private void lockMovement() {
        PlayerController.Instance.MoveLock = true;
    }
    private void unlockMovement() {
        PlayerController.Instance.MoveLock = false;
    }


    private void MouseFollowWithOffset() { // slashcollider follow
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;
        if (PlayerController.Instance.AttackDirectionLock == true) {return;}
        if (angle > 67.5 && angle <= 112.5) { // north
            SlashCollider.transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (angle > 22.5 && angle <= 67.5) { // northeast
            SlashCollider.transform.rotation = Quaternion.Euler(0, 0, 45);
        } else if (angle > -22.5 && angle <= 22.5) { // east
            SlashCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (angle > -67.5 && angle <= -22.5) { // southeast
            SlashCollider.transform.rotation = Quaternion.Euler(0, 0, -45);
        } else if (angle > -112.5 && angle <= -67.5) { // south
            SlashCollider.transform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (angle > -157.5 && angle <= -112.5) { // southwest
            SlashCollider.transform.rotation = Quaternion.Euler(0, 0, -135);
        } else if (angle > 112.5 && angle <= 157.5) { // northwest
            SlashCollider.transform.rotation = Quaternion.Euler(0, 0, 135);
        } else if (angle > 157.5 || angle < -157.5 ) { // west
            SlashCollider.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void SlashEffectFollowMouse() { // slashEffect follow mouse
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;
        if (angle > 67.5 && angle <= 112.5) { // north
                Clockwise = true;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (angle > 22.5 && angle <= 67.5) { // northeast
            Clockwise = false;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 45);
        } else if (angle > -22.5 && angle <= 22.5) { // east
            Clockwise = true;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
        } else if (angle > -67.5 && angle <= -22.5) { // southeast
            Clockwise = false;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, -45);
        } else if (angle > -112.5 && angle <= -67.5) { // south
            Clockwise = true;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (angle > -157.5 && angle <= -112.5) { // southwest
            Clockwise = true;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, -135);
        } else if (angle > 112.5 && angle <= 157.5) { // northwest
            Clockwise = true;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 135);
        } else if (angle > 157.5 || angle < -157.5 ) { // west
            Clockwise = true;
            slashEffectAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 180, 0);
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA3")) {
            Clockwise = !Clockwise;
        }
    }

    private void StabColliderFollow() { // Sprint attack stabcollider follow
       Vector2 vectorOne = stabMoveDirection;
       Vector2 vectorTwo = Vector2.zero; 
       float angle = Mathf.Atan2(vectorOne.y - vectorTwo.y, vectorOne.x - vectorTwo.x) * Mathf.Rad2Deg;
        if (angle > 67.5 && angle <= 112.5) { // north
            StabCollider.transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (angle > 22.5 && angle <= 67.5) { // northeast
            StabCollider.transform.rotation = Quaternion.Euler(0, 0, 45);
        } else if (angle > -22.5 && angle <= 22.5) { // east
            StabCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (angle > -67.5 && angle <= -22.5) { // southeast
            StabCollider.transform.rotation = Quaternion.Euler(0, 0, -45);
        } else if (angle > -112.5 && angle <= -67.5) { // south
            StabCollider.transform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (angle > -157.5 && angle <= -112.5) { // southwest
            StabCollider.transform.rotation = Quaternion.Euler(0, 0, -135);
        } else if (angle > 112.5 && angle <= 157.5) { // northwest
            StabCollider.transform.rotation = Quaternion.Euler(0, 0, 135);
        } else if (angle > 157.5 || angle < -157.5 ) { // west
            StabCollider.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void WeaponReset() {
        swordAnimHandler.ResetSwordAnim();
        isAttacking = false;
        secondaryAttack = false;
        shieldAction = false;
        myAnimator.SetBool(ISATTACKING_HASH, false);
        attackCounter = 0;
    }

}
