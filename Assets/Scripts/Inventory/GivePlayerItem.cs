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
       bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result == true) {
            Debug.Log("Item added");
        } else {
            Debug.Log("Inventory full");
        }
    }

    // public void GetSelectedItem() {
    //     Item receivedItem = inventoryManager.GetSelectedItem();
    // }
}
