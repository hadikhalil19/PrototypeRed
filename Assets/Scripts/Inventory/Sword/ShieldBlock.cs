using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : MonoBehaviour
{

    public bool shieldActive = false;
    public int shieldManaCost = 0;
    private KnockBack knockback;
    [SerializeField] private float ShieldKnockBackThrust = 2f;
    private void Awake() {
        knockback = GetComponent<KnockBack>();
    }

    public void TakeDamage(int damageAmount,  Transform hitTransform) {
        ScreenShakeManager.Instance.ShakeScreen();
        
        if (shieldActive && PlayerMana.Instance.CurrentMana > (shieldManaCost + damageAmount)) {
            //knockback.GetKnockedBack(hitTransform, ShieldKnockBackThrust);
            PlayerMana.Instance.UseMana(shieldManaCost + damageAmount);
            //GetComponent<Animator>().SetTrigger(SHILEDHIT_HASH);
            return;
        } else {
            PlayerHealth.Instance.TakeDamage(damageAmount, transform);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
       EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
       if (enemyHealth) {
        enemyHealth.attacksBlocked = true;
       }
    }
    private void OnTriggerExit2D(Collider2D other) {
       EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
       if (enemyHealth) {
        enemyHealth.attacksBlocked = false;
       }
    }

    
    

}
