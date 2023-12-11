using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    public Item item;

    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }

    public void InfoUpdate() {
        item = GetComponentInChildren<DragItem>().item;
        weaponInfo = item.weaponInfo;
    }

    public void InfoMakeEmpty() {
        item = null;
        weaponInfo = null;
    }
 
 
}
