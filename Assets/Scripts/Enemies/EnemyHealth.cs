using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [SerializeField] float knockBackForce = 10F;
    [SerializeField] float deathDelay = 1f;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] bool hasFlashAnim = true;
    [SerializeField] int staggerThreshold = 10;
    
    private int currentHealth;
    private KnockBack knockBack;
    private Flash flash;

    public bool dying = false;

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
    if(dying) {return;}
    if(knockBack.GettingKnockedBack) {return;}
    currentHealth -= damage;
    knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackForce);
    //enemyAnimController?.PlayHitAnim();
    if (enemyAnimController) { // if it has enemyAnimController and a built in flash
        int staggerValue = (damage%startingHealth)*100;
        Debug.Log(staggerValue);
        if (staggerValue >= staggerThreshold) {
            enemyAnimController?.PlayHitAnim();
            Debug.Log("stagger");
        }
        
    } 
    if (!hasFlashAnim) {
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
