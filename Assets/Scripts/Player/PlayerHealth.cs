// Player health and death logic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Proto.Saving;
using Proto.SceneManagement;
using UnityEngine.Events;
using Proto.UI;

public class PlayerHealth : Singleton<PlayerHealth>, ISaveable
{
    public bool IsDead;
    //public bool shieldActive = false;
    //public bool shieldCollison = false;
    //public int shieldManaCost = 0;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float knockBackThrustAmount = 5f;
    // [SerializeField] private float ShieldKnockBackThrust = 2f;
    // [SerializeField] float shieldBlockAngle = 90f;
    
    [SerializeField] private string deathSceneTransitionName;
    [SerializeField] UnityEvent takeDamageEvent;

    private PlayerController playerController;
    private ShowHideUI showHideUI;
    const string BOSSUI_TEXT = "BossUI";

    public float damageRecoveryTime = 0.5f;
    private int currentHealth;
    public bool canTakeDamage =  true;
    public bool rollInvulnerable = false;
    private KnockBack knockback;
    private Flash flash;
    private Slider healthSlider;
    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string CAMP_TEXT = "Camp";
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    readonly int RELOAD_HASH = Animator.StringToHash("Reload");
    readonly int SHILEDHIT_HASH = Animator.StringToHash("ShieldHit");

    readonly int PLAYERHIT_HASH = Animator.StringToHash("PlayerHit");


    protected override void Awake() {
        base.Awake();
        playerController = GetComponent<PlayerController>();
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start() {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void lockMovement() {
        playerController.MoveLock = true;
    }
    private void unlockMovement() {
        playerController.MoveLock = false;
    }

    private void lockAttack() {
        playerController.AttackLock = true;
    }
    private void unlockAttack() {
        playerController.AttackLock = false;
    }

// player collison damage logic that only works for older enemyAI with no A*. 
    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>(); // only works for older enemyAI with no A*
        if (enemy) {
            TakeDamage(1, other.transform);
        }

    }

// method for player healing and updating health bar.
    public void HealPlayer(int healValue) {
        if (currentHealth + healValue < maxHealth) {
            currentHealth += healValue;
            UpdateHealthSlider();
        } else {
            currentHealth = maxHealth;
            UpdateHealthSlider();
        }
    }

    // bool IsFacingTarget(Vector2 facingDirection, Transform target, float marginOfError)
    // {
    //     Vector2 directionToTarget = (target.position - transform.position).normalized;
    //     float angle = Vector2.SignedAngle(facingDirection, directionToTarget);

    //     // Check if the angle is within the margin of error
    //     // Debug.Log(facingDirection);
    //     // Debug.Log(directionToTarget);
    //     // Debug.Log(Mathf.Abs(angle));
    //     return Mathf.Abs(angle) <= marginOfError / 2;
    // }

// method for player taking damage and updating health bar. Also calls checkifplayerdeath.
// also checks if player is invulnerable from roll, and if so, does not take damage.

    public void TakeDamage(int damageAmount,  Transform hitTransform) {
        if (!canTakeDamage) { return; }
        if (IsDead) { return; }
        if (rollInvulnerable) { return; }
        ScreenShakeManager.Instance.ShakeScreen();
        
        takeDamageEvent.Invoke();
        canTakeDamage = false;
        currentHealth -= damageAmount;
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        //StartCoroutine(flash.FlashRoutine());
        if (currentHealth > 0) {
            PlayerHitStagger();
            StartCoroutine(DamageRecoveryRoutine());
        }
        
        
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

// method for checking if player is dead, and if so, calls deathloadsceneroutine.
    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0 && !IsDead) {
            IsDead = true;
            canTakeDamage = false;
            ActiveWeapon.Instance.disableAttack = true;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private void PlayerHitStagger() {
        GetComponent<Animator>().SetTrigger(PLAYERHIT_HASH);
        ActiveWeapon.Instance.WeaponReset();
        lockMovement();
        lockAttack();
    }

// method for initiating reloading scene on death. Calls UIFade and starts loadsceneroutine.
    private IEnumerator DeathLoadSceneRoutine() {
        yield return new WaitForSeconds(1f);
        //Destroy(gameObject);
        //SceneManager.LoadScene(CAMP_TEXT);
        SceneManagement.Instance.SetTransitionName(deathSceneTransitionName);
        UIFade.Instance.FadeIn(1);
        StartCoroutine(LoadSceneRoutine());
    }

// method for loading new scene on death. Resets player health, weapon, and camera.
    private IEnumerator LoadSceneRoutine() {
        yield return  new WaitForSeconds(3f);
        currentHealth = maxHealth;
        UpdateHealthSlider();
        GameOverReset();
        IsDead = false;
        //CameraController.Instance.SetPlayerCameraFollow();
        ActiveWeapon.Instance.disableAttack = false; 
        ActiveWeapon.Instance.WeaponReset();
        GetComponent<Animator>().SetTrigger(RELOAD_HASH);
        //UIFade.Instance.FadeOut();
        canTakeDamage = true;
        SceneManager.LoadScene(CAMP_TEXT);

    }


// makes player invulnerable for a short time after taking damage.
    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
        unlockMovement();
        unlockAttack(); 
    }

// updates health bar slider.
    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

// method for saving player health.
    public object CaptureState()
    {
        return currentHealth;
    }

// method for reloading player health.
    public void RestoreState(object state)
    {
        currentHealth = (int)state;
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

// method for resetting boss UI on playerdeath.
    private void GameOverReset() {
        showHideUI = GameObject.Find(BOSSUI_TEXT).GetComponent<ShowHideUI>();
        if (showHideUI) {
            showHideUI.SetVisibility(false);
        }
        
    }

}
