// ActiveInventory.cs is the script that is used to change the active weapon or item. This script is attached to the ActiveEquipment gameobject which is a child of the player gameobject.
// It keeps track of the player's input and calls the ToggleActiveHighlight function when the player presses the inventory slot buttons to change between active inventory.
// ChangeEquipedItem function is called when the player presses the inventory slot buttons to change between active inventory
// It also has a function to equip the starting weapon when the player spawns in the game.  

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singleton<ActiveInventory>
{
    public int activeSlotIndexNum = 0;
    private PlayerControls playerControls;
    
    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start() {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        //ToggleActiveHighlight(0);
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    // equip the starting weapon when the player spawns in the game.
    public void EquipStartingWeapon() {
        ToggleActiveHighlight(0);
    }

    // Toggle the active inventory slot based on the player's input and call the ToggleActiveHighlight function
    private void ToggleActiveSlot (int numValue) {
        if (!PlayerController.Instance.MoveLock && !PlayerController.Instance.AttackLock) {
            ToggleActiveHighlight(numValue -1);
        }
        
    }

    // Highlight the active inventory slot and call the ChangeEquipedItem function
    private void ToggleActiveHighlight(int indexNum) {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform) {
            inventorySlot.GetComponent<Image>().color = new Color32(100,100,100,100);
        }
        
        this.transform.GetChild(indexNum).GetComponent<Image>().color = new Color32(255,255,255,255);

        ChangeEquipedItem();
    }

    // Destroy the current active weapon or Item and spawn the new one based on the active inventory slot index
    public void ChangeEquipedItem() {
        if (PlayerHealth.Instance.IsDead) return;
        
        DestroyActiveItem();

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        
        if (inventorySlot.GetItemType() == 1)
        {
            ChangeActiveWeapon(inventorySlot);
        }
        if (inventorySlot.GetItemType() == 2)
        {
            ChangeActiveMiscItem(inventorySlot);
        }
        if (inventorySlot.GetItemType() == 10)
        {
            ActiveWeapon.Instance.WeaponNull();
        }


    }

    // Destroy active weapon or item
    private void DestroyActiveItem() {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null) {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }
        if (ActiveMiscItem.Instance.CurrentActiveMiscItem != null) {
            Destroy(ActiveMiscItem.Instance.CurrentActiveMiscItem.gameObject);
        }
    }

    // Spawn the new item based on the active inventory slot index and attach it to the ActiveEquipment gameobject as a child

    private void ChangeActiveMiscItem(InventorySlot inventorySlot)
    {
        ItemInfo itemInfo = inventorySlot.GetIteminfo();

        if (itemInfo == null)
        {
            ActiveMiscItem.Instance.MiscItemNull();
            return;
        }

        GameObject itemToSpawn = itemInfo.itemPrefab;
        GameObject newMiscItem = Instantiate(itemToSpawn, ActiveMiscItem.Instance.transform.position, Quaternion.identity);
        ActiveMiscItem.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        newMiscItem.transform.parent = ActiveMiscItem.Instance.transform;
        ActiveMiscItem.Instance.NewItem(newMiscItem.GetComponent<MonoBehaviour>());
    }

    private static void ChangeActiveWeapon(InventorySlot inventorySlot)
    {
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
