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

    public void IncrementCurrentGold() {
        currentGold += 1;

        if (goldText == null) {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
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
