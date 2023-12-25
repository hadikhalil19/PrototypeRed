// EconomyManager.cs manages the economy of the game. It keeps track of the current gold the player has and updates the UI accordingly.
// It is used for increasing and decreasing the gold amount the player is carrying.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Proto.Saving;

public class EconomyManager : Singleton<EconomyManager>, ISaveable
{
    private TMP_Text goldText;
    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";


// IncrementCurrentGold method is called when the player picks up a coin
// it increases the current gold amount by 1 and updates the UI
    public void IncrementCurrentGold() {
        currentGold += 1;

        if (goldText == null) {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }

// ConsumeGold method is called when the player buys an item
// it decreases the current gold amount by the amount cost of the item and updates the UI
    public void ConsumeGold(int amount) {
        currentGold -= amount;
        UpdateCurrentGold();
    }

    public int GetCurrentGold() {
        return currentGold;
    }

    private void UpdateCurrentGold() {
        if (goldText == null) {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        goldText.text = currentGold.ToString("D3");
    }


     public object CaptureState()
    {
        return currentGold;
    }

    public void RestoreState(object state)
    {
        currentGold = (int)state;
        UpdateCurrentGold();
        
    }

}
