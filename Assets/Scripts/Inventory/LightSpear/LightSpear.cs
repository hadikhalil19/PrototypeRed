using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightSpear : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spearSpawnPoint;
    [SerializeField] UnityEvent spearThrowStartEvent;
    private Vector3 lockedSpearSpawnPosition;
    private Quaternion lockedSpearSpawnRotation;
    private Animator myAnimator;

    private SpearAnimHandler spearAnimHandler;
    private bool lightSpearAttacking;

    


    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int SECONDARY_HASH = Animator.StringToHash("Secondary");
    readonly int LIGHTSPEARATTACKING_HASH = Animator.StringToHash("LightSpearAttacking");

    private void Awake() {
        myAnimator = PlayerController.Instance.GetComponent<Animator>();
        spearAnimHandler = FindObjectOfType<SpearAnimHandler>();   
    }

    private void FixedUpdate() {
        LightSpearAnimEnd();
        LockSpawnPoint();
        ToggleSpearRlease();
        
    }

    public void Attack() {
        if (PlayerController.Instance.AttackLock) {return;}
        if (PlayerMana.Instance.CurrentMana < weaponInfo.weaponManaCost) {return;} 
        if (lightSpearAttacking) {return;}
        myAnimator.SetBool(LIGHTSPEARATTACKING_HASH, true);
        myAnimator.SetTrigger(ATTACK_HASH);
        PlayerController.Instance.AttackMoving = true;
        lightSpearAttacking = true;
        lockMovement();
        spearThrowStartEvent.Invoke();
    }

    public void SecondaryAttackStart() {
        //Debug.Log("LightSpear SecondaryAttack");
    }
    public void SecondaryAttackStop() {
        
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

    private void ToggleSpearRlease() {
        if (spearAnimHandler.GetSpearRelease) {
            GameObject newArrow = Instantiate(projectilePrefab, lockedSpearSpawnPosition, lockedSpearSpawnRotation);
            newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
            newArrow.GetComponent<Projectile>().UpdateProjectileDamage(weaponInfo.weaponDamage);
            spearAnimHandler.GetSpearRelease = false;
            PlayerMana.Instance.UseMana(weaponInfo.weaponManaCost);
        }
        
        
    }

    private void LockSpawnPoint() {
        if (!spearAnimHandler.GetThrowing) {
            lockedSpearSpawnPosition = spearSpawnPoint.position;
            lockedSpearSpawnRotation = spearSpawnPoint.rotation;
        }
    }

    private void LightSpearAnimEnd() {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SideLightSpear")) 
        {
            lightSpearAttacking = false;
            myAnimator.SetBool(LIGHTSPEARATTACKING_HASH, false);
            unlockMovement();
        }

    }

    public void WeaponReset() {
        lightSpearAttacking = false;
        myAnimator.SetBool(LIGHTSPEARATTACKING_HASH, false);
    }
}
