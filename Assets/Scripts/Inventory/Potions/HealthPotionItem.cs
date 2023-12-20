// Main script for handling Health Potion Item, its usage and player animation. This script is attached to the Health Potion Item prefab.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthPotionItem : MonoBehaviour, IMiscItem
{
   [SerializeField] private ItemInfo itemInfo;

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


    // Use active item when not over UI and not attacking and not drinking potion. 
    public void UseActiveItem(){
        if (EventSystem.current.IsPointerOverGameObject()) {return;}
        if (PlayerController.Instance.AttackLock) {return;}
        if (isDrinkingPotion) {return;}
        isDrinkingPotion = true;

        myAnimator.SetBool(DRINKINGPOTION_HASH, true);
        myAnimator.SetTrigger(ATTACK_HASH);

        lockMovement();

        PlayerHealth.Instance.HealPlayer(healAmount);
        Item item = inventoryManager.GetSelectedItem(potionsAreConsumable); // if item is consumable stack count will be decrease by 1
        
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
    public ItemInfo GetItemInfo(){
        return itemInfo;

    }

    // Check if the drink potion animation is done and manage the animator and unlock movement and also consume the item if it is the last stack
    private void DrinkPotionAnimEnd() {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("DrinkPotion")) 
        {
            isDrinkingPotion = false;
            myAnimator.SetBool(DRINKINGPOTION_HASH, false);
            unlockMovement();
            if (inventoryManager.lastStackUsed) {
                ConsumeActiveItem();
                inventoryManager.lastStackUsed = false;
            }
        }

    }

    //Reset the item usage for player animator
    // this is temp and needs to be changed with item info instead of weaponinfo
    public void ItemReset() {
        isDrinkingPotion = false;
        myAnimator.SetBool(DRINKINGPOTION_HASH, false);
    }
    
    // consume the active item and destroy it and reset the Item usage and unlock movement
    private void ConsumeActiveItem() {
        unlockMovement();
        ItemReset();
        if (ActiveMiscItem.Instance.CurrentActiveMiscItem != null) {
            Destroy(ActiveMiscItem.Instance.CurrentActiveMiscItem.gameObject);
        }
    }
}
