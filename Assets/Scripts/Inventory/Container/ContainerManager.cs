// manages the container UI and the container itself
// stores container items and updates the UI itemslots accordingly

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    public InventorySlot [] inventorySlots;
    public Item [] itemsInContainer;
    public GameObject equipedItemPrefab;
    public bool lastStackUsed = false;
    

    // InitialiizeContainer method is called when the container is opened
    // it initializes the container inventory slots using itemsInContainer array
    // it then updates the inventory slot info for each slot using additem method
    public void InitialiizeContainer() {
        for(int i = 0; i < itemsInContainer.Length; i++) {
            Item item = itemsInContainer[i];
            if (item != null) {
                bool itemAdded = AddItem(item);
                Debug.Log("Item added: " + itemAdded);
            }
        }

    }

    // inventorySlots hold a reference to "InventorySlot slot" for container inventory slots in an array
    // AddItem method takes a given item and checks if the item is stackable and if the item is already in the Container inventory slots
    // if the item is stackable and already in the inventory, it increases the stack count of the item
    // if the item is not in the inventory, it spawns a new item in the inventory using SpawnNewItem method
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

    // SpawnNewItem method takes a given item SObject and a given inventory slot and instantiates a new item in the inventory
    // for this it needs to first create the empty equipedItem game object using the equipedItemPrefab with the slot as its parent and then get the DragItem component from it
    // it then initializes the dragitem with the given item and updates the inventory slot info
    private void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItemGo = Instantiate(equipedItemPrefab, slot.transform);
        DragItem dragItem = newItemGo.GetComponent<DragItem>();
        
        dragItem.InitialiizeItem(item);
        slot.InfoUpdate();
    }

    // GetSelectedItem method takes a bool consumeItem and returns the item in the active inventory slot
    // consumeItem is used to determine if the item should be consumed or not
    // if consumeItem is true, it decreases the stack count of the item in the slot and if the stack count is less than 1, it destroys the item and makes the slot empty
    // it then returns the item
    // GetSelectedItem only works for the current active inventory slot
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
                    slot.InfoMakeEmpty();
                    lastStackUsed = true;
                    //ActiveInventory activeInventory = slot.GetComponentInParent<ActiveInventory>();
                    //activeInventory.ChangeEquipedItem();
                    // 
                } else {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        } else {
            return null;
        }
    }
}
