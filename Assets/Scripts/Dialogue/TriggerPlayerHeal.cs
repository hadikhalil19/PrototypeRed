using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerHeal : MonoBehaviour
{
    void TriggerHeal() {
        PlayerHealth.Instance.HealPlayer();
    }
    
    private void Update() {
        TriggerHeal();
    }
}
