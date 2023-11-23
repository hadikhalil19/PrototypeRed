using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerDamage : MonoBehaviour
{
    [SerializeField] int damageAmount = 1;
    private EnemyHealth myHealth;

    private void Awake() {
        myHealth = GetComponentInParent<EnemyHealth>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        ShieldBlock shieldBlock = other.gameObject.GetComponentInChildren<ShieldBlock>();

        if (myHealth.attacksBlocked) {
            if (shieldBlock) {
                
                shieldBlock.TakeDamage(damageAmount, transform);
            }     
        } else if(playerHealth) {
            playerHealth.TakeDamage(damageAmount, transform);    
            
                
            
        }
    }
}
