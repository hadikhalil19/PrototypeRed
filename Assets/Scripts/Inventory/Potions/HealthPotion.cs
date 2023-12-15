using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthPotion : MonoBehaviour, IWeapon
{
   [SerializeField] private WeaponInfo weaponInfo;

   [SerializeField] int healAmount = 50;

   private InventoryManager inventoryManager;

   private bool potionsAreConsumable = true;

   private Animator myAnimator;

   private bool isDrinkingPotion = false;

   readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int DRINKINGPOTION_HASH = Animator.StringToHash("DrinkingPotion");

   private void Awake() {
        myAnimator = PlayerController.Instance.GetComponent<Animator>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void FixedUpdate() {
        DrinkPotionAnimEnd(); // this is temp and needs to be changed with AnimEvents once there is a proper potion drink animation
    }

    public void Attack(){
        if (EventSystem.current.IsPointerOverGameObject()) {return;}
        if (PlayerController.Instance.AttackLock) {return;}
        if (isDrinkingPotion) {return;}
        isDrinkingPotion = true;

        myAnimator.SetBool(DRINKINGPOTION_HASH, true);
        myAnimator.SetTrigger(ATTACK_HASH);

        lockMovement();

        PlayerHealth.Instance.HealPlayer(healAmount);
        Item item = inventoryManager.GetSelectedItem(potionsAreConsumable);
        // item can be used to do something if not consumable
        // if (item != null) {
        //     PlayerMana.Instance.UseMana(weaponInfo.weaponManaCost); 
        // }
    }

    private void lockMovement() {
        PlayerController.Instance.MoveLock = true;
    }
    private void unlockMovement() {
        PlayerController.Instance.MoveLock = false;
    }

    public void SecondaryAttackStart(){}
    public void SecondaryAttackStop(){}
    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;

    }
    private void DrinkPotionAnimEnd() {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("DrinkPotion")) 
        {
            isDrinkingPotion = false;
            myAnimator.SetBool(DRINKINGPOTION_HASH, false);
            unlockMovement();
        }

    }

    public void WeaponReset() {
        isDrinkingPotion = false;
        myAnimator.SetBool(DRINKINGPOTION_HASH, false);
    }
}
