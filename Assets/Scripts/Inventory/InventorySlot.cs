// Info: InventorySlot class that holds and updates information about the items equiped in the inventory slots

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private ItemInfo itemInfo;
    public Item item;

    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }

    public ItemInfo GetIteminfo() {
        return itemInfo;
    }

    // update the info of the item in the slot according to the item type 
    public void InfoUpdate() {
        item = GetComponentInChildren<DragItem>().item;
        if (item) {
            if (item.itemType == ItemType.Tool) {
                itemInfo = item.itemInfo;
                weaponInfo = null;
            } else if (item.itemType == ItemType.Weapon) {
                weaponInfo = item.weaponInfo;
                itemInfo = null;
            } else if (item.itemType == ItemType.Misc) {
                itemInfo = item.itemInfo;
                weaponInfo = null;
            } else {
                itemInfo = null;
                weaponInfo = null;
            }
        } else {
            weaponInfo = null;
            itemInfo = null;
        }
        
    }

    public void InfoMakeEmpty() {
        item = null;
        weaponInfo = null;
        itemInfo = null;
    }
    
    public int GetItemType() {
        if (item == null) { 
            Debug.Log("Item was null");
            return 10;
            }
        if (item.itemType == ItemType.Tool) {
            return 0;
        } else if (item.itemType == ItemType.Weapon) {
            return 1;
        } else if (item.itemType == ItemType.Misc) {
            return 2;
        } else {
            return 10;
        }
        
    }
 
}
