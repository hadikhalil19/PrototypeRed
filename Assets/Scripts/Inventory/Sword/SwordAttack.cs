using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour, IWeapon
{
    
    private Animator myAnimator;
    [SerializeField] int attackCounter = 0;

    [SerializeField] int numberOfAttacks = 2;
    private bool isAttacking = false;
    public bool Clockwise  {get; private set;}
    private bool hasSlashEffect = false;

    //private bool secondaryAttack = false;
    [SerializeField] private Transform SlashCollider;

    private SwordAnimHandler swordAnimHandler;
    
    [SerializeField] private GameObject slashEffectPrefab;

    [SerializeField] private Transform slashEffectSpawn;

    private GameObject slashEffectAnim;
    [SerializeField] private WeaponInfo weaponInfo;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int SECONDARY_HASH = Animator.StringToHash("Secondary");
    readonly int SHIELDUP_HASH = Animator.StringToHash("ShieldUp");
    readonly int ATTACKCounter_HASH = Animator.StringToHash("AttackCounter");
    readonly int ISATTACKING_HASH = Animator.StringToHash("isAttacking");
    //readonly int SWORDA1_HASH = Animator.StringToHash("SwordA1");
    readonly int SWORDA2_HASH = Animator.StringToHash("SwordA2");
    readonly int SWORDA3_HASH = Animator.StringToHash("SwordA3");

    private void Awake() {
        myAnimator = PlayerController.Instance.GetComponent<Animator>();
        swordAnimHandler = FindObjectOfType<SwordAnimHandler>();
    }

    
    private void Update() {
        MouseFollowWithOffset();
    }

    private void FixedUpdate() {
        AttackAnimEnd();
        toggleSlashHitbox();
    }

    public void Attack() {
        if (PlayerController.Instance.AttackLock) {return;}
        if (PlayerMana.Instance.CurrentMana < weaponInfo.weaponManaCost) {return;}
       
        myAnimator.SetBool(SHIELDUP_HASH, false);
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

    public void SecondaryAttackStart() {
        if (PlayerController.Instance.AttackLock) {return;}
        if (isAttacking) { return;}
        //secondaryAttack = true;
        myAnimator.SetTrigger(SECONDARY_HASH);
        myAnimator.SetBool(SHIELDUP_HASH, true);
        lockMovement();
    }

    public void SecondaryAttackStop() {
        if (isAttacking) { return;}
        //secondaryAttack = false;
        myAnimator.SetBool(SHIELDUP_HASH, false);
        unlockMovement();
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
        } else if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordA3")) 
        {
            isAttacking = false;
            unlockMovement();
            myAnimator.SetBool(ISATTACKING_HASH, false);
            resetCurrentAttackCounter();
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


}
