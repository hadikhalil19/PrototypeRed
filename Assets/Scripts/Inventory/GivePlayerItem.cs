using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickUpItem(int id) {
        inventoryManager.AddItem(itemsToPickup[id]);
    }
}
