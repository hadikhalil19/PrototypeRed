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

    public void InfoUpdate() {
        item = GetComponentInChildren<DragItem>().item;
        if (item) {
            weaponInfo = item.weaponInfo;
        } else {
            weaponInfo = null;
        }
        
    }

    public void InfoMakeEmpty() {
        item = null;
        weaponInfo = null;
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
