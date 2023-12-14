using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot [] inventorySlots;
    public GameObject equipedItemPrefab;
    
    public bool AddItem(Item item) {
        
        // check if any slot has the same item with count lower than max
         for(int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            DragItem itemInSlot = slot.GetComponentInChildren<DragItem>();
            
            if (itemInSlot != null && itemInSlot.item.stackable && itemInSlot.item == item && itemInSlot.stackCount < itemInSlot.item.maxStack) {
                itemInSlot.stackCount++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        // look for empty slot and then spawn Equiped Item in it and add the item ScriptableObject to the equiped item gameobject
        for(int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            DragItem itemInSlot = slot.GetComponentInChildren<DragItem>();
            
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    private void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItemGo = Instantiate(equipedItemPrefab, slot.transform);
        DragItem dragItem = newItemGo.GetComponent<DragItem>();
        dragItem.InitialiizeItem(item);
    }

    public Item GetSelectedItem(bool consumeItem) {
        int activeSlotIndexNum = ActiveInventory.Instance.activeSlotIndexNum;
        InventorySlot slot = inventorySlots[activeSlotIndexNum];
        DragItem itemInSlot = slot.GetComponentInChildren<DragItem>();
        if (itemInSlot != null) {
            Item item = itemInSlot.item;
            if (consumeItem == true) {
                itemInSlot.stackCount--;
                if (itemInSlot.stackCount < 1) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        } else {
            return null;
        }
    }

    // public void ConsumeItem() {

    // }
}
