using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.Audio;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 5;
    [SerializeField] float knockBackForce = 10F;
    [SerializeField] float deathDelay = 1f;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] bool hasFlashAnim = true;
    [SerializeField] int staggerThreshold = 10;
    [SerializeField] int attackInteruptThreshold = 50;
    [SerializeField] bool bossEnemy = false;

    [SerializeField] FloatingHealthBar floatingHealthBar;
    private BossUI bossUI;
    
    private int currentHealth;
    private KnockBack knockBack;
    private Flash flash;
    private bool healthbarVisible = false;

    public bool dying = false;

    private EnemyAnimController enemyAnimController;
    private GenericAudioPlayer genericAudioPlayer;

    private void Awake() {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
        genericAudioPlayer = GetComponentInChildren<GenericAudioPlayer>();
        floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
        bossUI = GetComponent<BossUI>();
    }

   private void Start() {
    currentHealth = startingHealth;
    if(floatingHealthBar) {
        floatingHealthBar.UpdateFloatingHealthBar(currentHealth, startingHealth);
    }
   }

   public void TakeDamage(int damage) {
    if(dying) {return;}
    if(knockBack.GettingKnockedBack) {return;}
    currentHealth -= damage;
    knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackForce);
    if(floatingHealthBar) {
        floatingHealthBar.UpdateFloatingHealthBar(currentHealth, startingHealth);
    }
    if(bossUI && bossEnemy) {
        bossUI.UpdateBossHealthBar(currentHealth, startingHealth);
    }
    if (genericAudioPlayer) {
        genericAudioPlayer.PlayRandomAudioClip();
    }
    if (enemyAnimController) { // if it has enemyAnimController and a built in flash
        int staggerValue = (int)(damage * 100.0/startingHealth);
        if (staggerValue >= staggerThreshold) {
            enemyAnimController?.PlayHitAnim();
            
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
        if(bossUI && bossEnemy) {bossUI.SetActiveState(false);}
        //Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        GetComponent<PickUpSpawner>().DropItems();
        Destroy(gameObject); 
    }
}
