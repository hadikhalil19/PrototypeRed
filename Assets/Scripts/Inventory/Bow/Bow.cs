using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    private bool nockArrowAim = false;
    private bool bowAttacking = false;
    private Animator myAnimator;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    [SerializeField] private WeaponInfo weaponInfo;

    private ArrowAnimHandler arrowAnimHandler;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int SECONDARY_HASH = Animator.StringToHash("Secondary");

    readonly int BOWATTACK_HASH = Animator.StringToHash("BowAttack");
    readonly int SHOOTARROW_HASH = Animator.StringToHash("ShootArrow");
    readonly int NOCKINGARROW_HASH = Animator.StringToHash("NockingArrow");
    readonly int BOWAIM_HASH = Animator.StringToHash("BowAim");
    readonly int RELOADARROW_HASH = Animator.StringToHash("ReloadArrow");
    //readonly int SWORDA1_HASH = Animator.StringToHash("SwordA1");
    //readonly int SWORDA1_HASH = Animator.StringToHash("SwordA1");

    private void Awake() {
        myAnimator = PlayerController.Instance.GetComponent<Animator>();
        arrowAnimHandler = FindObjectOfType<ArrowAnimHandler>();
       
    }

    private void FixedUpdate() {
        ArrowAnimEnd();
        playerAimingBow();
        ToggleArrowRlease();
    }


    public void Attack() {
        if (PlayerController.Instance.AttackLock) {return;}
        if (PlayerMana.Instance.CurrentMana < weaponInfo.weaponManaCost) {return;}
        if (!bowAttacking && (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("NockArrow") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ReloadArrow"))) {
        
        myAnimator.SetTrigger(ATTACK_HASH);
        myAnimator.SetBool(BOWATTACK_HASH, true);
        bowAttacking = true;
        PlayerController.Instance.AttackMoving = true;
        myAnimator.SetBool(SHOOTARROW_HASH, true);
        }
        

    }

    public void SecondaryAttackStart() {
        if (PlayerController.Instance.AttackLock) {return;}
        
        myAnimator.SetTrigger(SECONDARY_HASH);
        myAnimator.SetBool(NOCKINGARROW_HASH, true);
        myAnimator.SetBool(BOWAIM_HASH, true);
        nockArrowAim = true;
    }

    public void SecondaryAttackStop() {
        //myAnimator.SetBool(NOCKINGARROW_HASH, false);
        myAnimator.SetBool(BOWATTACK_HASH, false);
        myAnimator.SetBool(BOWAIM_HASH, false);
        myAnimator.SetBool(RELOADARROW_HASH, false);
        myAnimator.ResetTrigger(ATTACK_HASH);
        nockArrowAim = false;
        bowAttacking = false;
    }
    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }


    private void lockMovement() {
        PlayerController.Instance.MoveLock = true;
    }
    private void unlockMovement() {
        PlayerController.Instance.MoveLock = false;
    }

    private void playerAimingBow() {
        if (nockArrowAim || bowAttacking) {
            lockMovement();

        } else if (!PlayerController.Instance.AttackLock) {
            unlockMovement();
        }
    }

    private void ToggleArrowRlease() {
        
        if(arrowAnimHandler.GetArrowRelease) {
            arrowAnimHandler.GetArrowRelease = false;
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
            newArrow.GetComponent<Projectile>().UpdateProjectileDamage(weaponInfo.weaponDamage);
            PlayerMana.Instance.UseMana(weaponInfo.weaponManaCost);
        }
    }

    private void ArrowAnimEnd() {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("NockArrow")) 
        {
            myAnimator.SetBool(NOCKINGARROW_HASH, false);
           
        } else if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShootArrow")) 
        {
            myAnimator.SetBool(SHOOTARROW_HASH, false);
            //unlockMovement();
            bowAttacking = false;
            myAnimator.SetBool(BOWATTACK_HASH, false);
            if (nockArrowAim) {
                myAnimator.SetBool(RELOADARROW_HASH, true);
            } else {
                myAnimator.SetBool(BOWAIM_HASH, false);
            }
        } else if  (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ReloadArrow")) {
            myAnimator.SetBool(RELOADARROW_HASH, false);
            
        }

    }

    private void OnDestroy() {
        myAnimator.SetBool(SHOOTARROW_HASH, false);
        myAnimator.SetBool(BOWATTACK_HASH, false);
        myAnimator.SetBool(RELOADARROW_HASH, false);
        myAnimator.SetBool(NOCKINGARROW_HASH, false);
        myAnimator.ResetTrigger(ATTACK_HASH);
    }


}
