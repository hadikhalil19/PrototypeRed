using System.Collections;
using System.Collections.Generic;
using Proto.SceneManagement;
using UnityEngine;

public class GivePlayerItem : MonoBehaviour
{
    private InventoryManager inventoryManager;
    //private EconomyManager economyManager;
    public Item[] itemsToPickup;

    private void Awake() {
        inventoryManager = FindObjectOfType<InventoryManager>();
        //economyManager = FindObjectOfType<EconomyManager>();
    }

    private bool PayItemGoldCost(int id) {
        int itemCost = itemsToPickup[id].itemInfo.itemGoldCost;
        if (EconomyManager.Instance.GetCurrentGold() < itemCost) {
            Debug.Log("Not enough gold. Current gold is: " + EconomyManager.Instance.GetCurrentGold() + " and required gold to buy item is: " + itemCost + ".");
            return false;
        } else {
            EconomyManager.Instance.ConsumeGold(itemCost);
            Debug.Log("Gold spent: " + itemCost + " and current gold is: " + EconomyManager.Instance.GetCurrentGold());
            return true;
        }
    }

    public void PickUpItem(int id) {
       bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result == true) {
            Debug.Log("Item added");
        } else {
            Debug.Log("Inventory full");
        }
    }

// BuyItem method is called when the player clicks on the buy item diaglog button at the shop
// it first calls PayItemGoldCost method to check if the player has enough gold to buy the item and if so, it calls PickUpItem method to add the item to the inventory

    public void BuyItem(int id) {
        bool hasEnoughGold = PayItemGoldCost(id);
        if (hasEnoughGold == true) {
            PickUpItem(id);
        }
       
    }

    // public void GetSelectedItem() {
    //     Item receivedItem = inventoryManager.GetSelectedItem();
    // }
}
