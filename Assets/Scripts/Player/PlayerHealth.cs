using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Proto.Saving;
using Proto.SceneManagement;
using UnityEngine.Events;

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


    protected override void Awake() {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start() {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>(); // only works for older enemyAI with no A*
        if (enemy) {
            TakeDamage(1, other.transform);
        }

    }

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

    public void TakeDamage(int damageAmount,  Transform hitTransform) {
        if (!canTakeDamage) { return; }
        if (IsDead) { return; }
        if (rollInvulnerable) { return; }
        ScreenShakeManager.Instance.ShakeScreen();
        
        takeDamageEvent.Invoke();
        canTakeDamage = false;
        currentHealth -= damageAmount;
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());

        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0 && !IsDead) {
            IsDead = true;
            canTakeDamage = false;
            ActiveWeapon.Instance.disableAttack = true;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine() {
        yield return new WaitForSeconds(1f);
        //Destroy(gameObject);
        //SceneManager.LoadScene(CAMP_TEXT);
        SceneManagement.Instance.SetTransitionName(deathSceneTransitionName);
        UIFade.Instance.FadeIn();
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine() {
        yield return  new WaitForSeconds(3f);
        currentHealth = maxHealth;
        UpdateHealthSlider();
        IsDead = false;
        //CameraController.Instance.SetPlayerCameraFollow();
        ActiveWeapon.Instance.disableAttack = false; 
        ActiveWeapon.Instance.WeaponReset();
        GetComponent<Animator>().SetTrigger(RELOAD_HASH);
        //UIFade.Instance.FadeOut();
        canTakeDamage = true;
        SceneManager.LoadScene(CAMP_TEXT);

    }



    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public object CaptureState()
    {
        return currentHealth;
    }

    public void RestoreState(object state)
    {
        currentHealth = (int)state;
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

}
