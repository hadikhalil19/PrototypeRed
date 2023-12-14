using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour, IWeapon
{
   [SerializeField] private WeaponInfo weaponInfo;

   [SerializeField] int healAmount = 30;

   private InventoryManager inventoryManager;

   private bool potionsAreConsumable = true;

   private void Awake() {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void Attack(){
        PlayerHealth.Instance.HealPlayer(healAmount);
        Item item = inventoryManager.GetSelectedItem(potionsAreConsumable);
        // item can be used to do something if not consumable
        // if (item != null) {
        //     PlayerMana.Instance.UseMana(weaponInfo.weaponManaCost); 
        // }
    }
    public void SecondaryAttackStart(){}
    public void SecondaryAttackStop(){}
    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;

    }
    public void WeaponReset(){}
}
