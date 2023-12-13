using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot [] inventorySlots;
    public GameObject equipedItemPrefab;
    
    public void AddItem(Item item) {
        // look for empty slot and then spawn Equiped Item in it and add the item ScriptableObject to the equiped item gameobject
        for(int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            DragItem itemInSlot = slot.GetComponentInChildren<DragItem>();
            
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return;
            }
        }

    }

    private void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItemGo = Instantiate(equipedItemPrefab, slot.transform);
        DragItem dragItem = newItemGo.GetComponent<DragItem>();
        dragItem.InitialiizeItem(item);
    }
}
