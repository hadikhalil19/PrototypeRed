using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] float knockBackForce = 10F;
    [SerializeField] float deathDelay = 0.2f;
    [SerializeField] private GameObject deathVFXPrefab;
    
    private int currentHealth;
    private KnockBack knockBack;
    private Flash flash;

    private bool dying = false;

    private EnemyAnimController enemyAnimController;

    private void Awake() {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
    }

   private void Start() {
    currentHealth = startingHealth;
   }

   public void TakeDamage(int damage) {
    currentHealth -= damage;
    knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackForce);
    //enemyAnimController?.PlayHitAnim();
    if (enemyAnimController) {
        enemyAnimController?.PlayHitAnim();
    } else {
        StartCoroutine(flash.FlashRoutine());
    }
    
    DetectDeath();
   }

    private void DetectDeath() {
         if (currentHealth <= 0 && !dying) {
           StartCoroutine(DeathRoutine());
           dying = true;    
        }
    }
    public IEnumerator DeathRoutine() {
        enemyAnimController?.PlayDeathAnim();
        yield return new WaitForSeconds(deathDelay);
        //Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        GetComponent<PickUpSpawner>().DropItems();
        Destroy(gameObject); 
    }
}
