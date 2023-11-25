using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : MonoBehaviour
{

    public bool shieldActive = false;
    public int shieldManaCost = 0;
    private KnockBack knockback;
    [SerializeField] private float ShieldKnockBackThrust = 2f;
    readonly int SHILEDHIT_HASH = Animator.StringToHash("ShieldHit");
    private void Awake() {
        knockback = GetComponentInParent<KnockBack>();
    }

    public void TakeDamage(int damageAmount,  Transform hitTransform) {
        ScreenShakeManager.Instance.ShakeScreen();
        
        if (shieldActive && PlayerMana.Instance.CurrentMana > (shieldManaCost + damageAmount)) {
            //knockback.GetKnockedBack(hitTransform, ShieldKnockBackThrust);
            PlayerMana.Instance.UseMana(shieldManaCost + damageAmount);
            GetComponentInParent<Animator>().SetTrigger(SHILEDHIT_HASH);
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


    // private void OnCollisonEnter2D(Collider2D other) {
    //    EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
    //    Debug.Log("collision");
    //     if (enemyHealth) {
    //         Debug.Log("enemy collision");
    //         enemyHealth.attacksBlocked = true;
    //    }
    // }
    // private void OnCollisonExit2D(Collider2D other) {
    //     EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
    //     if (enemyHealth) {
    //         Debug.Log("enemy collision end");
    //         enemyHealth.attacksBlocked = false;
    //     }
    // }


    
    

}
