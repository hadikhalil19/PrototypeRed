using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerDamage : MonoBehaviour
{
    [SerializeField] int damageAmount = 1;
    private EnemyHealth myHealth;
    private Grape grape;
    [SerializeField] private bool playerCollision;  // playerCollison = true means it has already collided

    private void Awake() {
        myHealth = GetComponentInParent<EnemyHealth>();
        grape = GetComponentInParent<Grape>();
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (playerCollision) {return;}
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        ShieldBlock shieldBlock = other.gameObject.GetComponent<ShieldBlock>();

        // if (myHealth.attacksBlocked) {
        //     if (shieldBlock) {
                
        //         shieldBlock.TakeDamage(damageAmount, transform);
        //     }     
        // } else if(playerHealth) {
        //     playerHealth.TakeDamage(damageAmount, transform);         
            
        // }
        if (shieldBlock) {
                shieldBlock.TakeDamage(damageAmount, transform);
                playerCollision = true;
                Debug.Log("shield");
                grape.DisableMeleeCollider();
                //StartCoroutine(CollisionReloadRoutine(0.36f));
        } else if(playerHealth) {
                playerHealth.TakeDamage(damageAmount, transform);
                playerCollision = true;
                Debug.Log("player");
                //StartCoroutine(CollisionReloadRoutine(0.36f));
        }
    }

    private IEnumerator CollisionReloadRoutine(float cooldown) {
        yield return new WaitForSeconds(cooldown);
        playerCollision = false;
    }

     void OnDisable()
    {
        
        playerCollision = false;
        
    }

    void OnEnable()
    {
        playerCollision = false;
    }
}
