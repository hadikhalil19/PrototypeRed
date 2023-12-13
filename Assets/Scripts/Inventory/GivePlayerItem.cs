using System.Collections;
using System.Collections.Generic;
using Proto.SceneManagement;
using UnityEngine;

public class GivePlayerItem : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    private void Awake() {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void PickUpItem(int id) {
        inventoryManager.AddItem(itemsToPickup[id]);
        
    }
}
