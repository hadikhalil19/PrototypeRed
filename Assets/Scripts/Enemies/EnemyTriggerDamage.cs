using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerDamage : MonoBehaviour
{
    [SerializeField] int damageAmount = 1;
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damageAmount, transform);
    }
}
