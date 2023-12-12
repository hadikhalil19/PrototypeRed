using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;
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

    public void EquipStartingWeapon() {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot (int numValue) {
        if (!PlayerController.Instance.MoveLock && !PlayerController.Instance.AttackLock) {
            ToggleActiveHighlight(numValue -1);
        }
        
    }

    private void ToggleActiveHighlight(int indexNum) {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform) {
            //inventorySlot.GetChild(0).gameObject.SetActive(false);
            inventorySlot.GetComponent<Image>().color = new Color32(100,100,100,100);
        }

        //this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(indexNum).GetComponent<Image>().color = new Color32(255,255,255,255);

        ChangeEquipedItem();
    }

    public void ChangeEquipedItem() {
        if (PlayerHealth.Instance.IsDead) return;
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null) {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

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

    private void ChangeActiveMiscItem(InventorySlot inventorySlot)
    {
        ItemInfo itemInfo = inventorySlot.GetIteminfo();
        ActiveWeapon.Instance.WeaponNull();
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
