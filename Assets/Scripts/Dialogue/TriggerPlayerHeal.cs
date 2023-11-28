using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerHeal : MonoBehaviour
{
    [SerializeField] int healAmount = 30;
    public void TriggerHeal() {
        PlayerHealth.Instance.HealPlayer(healAmount);
    }
    
    // private void Update() {
    //     TriggerHeal();
    // }
}
