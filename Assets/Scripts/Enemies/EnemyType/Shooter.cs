using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0,359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBurst;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [Tooltip("Stagger has to be enabled for oscillate to work")]
    [SerializeField] private bool oscillate;
    [SerializeField] AudioSource spitAudio;

    private bool isShooting = false;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimController enemyAnimController;
    private ASEnemyAI aSEnemyAI;
    Rigidbody2D rb;

    [SerializeField] float speed = 200f;

    private bool attackMove = false;

    private Vector2 attackDirection;
    private bool playerCollision;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
        rb = GetComponent<Rigidbody2D>();
        aSEnemyAI = GetComponent<ASEnemyAI>();
    }

    private void OnValidate() {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if (burstCount < 1) { burstCount = 1; }
        if (timeBetweenBurst < 0.1f) { timeBetweenBurst = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
   
    }

    public void Attack() {
        if (!isShooting) {
            StartCoroutine(ShootRoutine());
            spitAudio.Play();
        }
    }
    
    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0.1f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger) { timeBetweenBurst = timeBetweenBurst / projectilesPerBurst; }

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate) {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
                    
            } 
            
            if (oscillate && i % 2 != 1 ) {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate) {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }
            
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.Euler(0,0,currentAngle));
                //newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) {yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            currentAngle = startAngle;
            
            if (!stagger) {
                yield return new WaitForSeconds(timeBetweenBurst);
            }
            
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0f;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle) {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector2 pos = new Vector2(x, y);
        return pos;
    }

    
    public void SecondaryAttack() {
        enemyAnimController?.PlayAttackAnim();
        myAnimator.SetFloat("idleX", transform.position.x - PlayerController.Instance.transform.position.x );
        myAnimator.SetFloat("idleY", PlayerController.Instance.transform.position.y - transform.position.y);
    }
    public void AttackAnimEvent() {
        attackDirection = ((Vector2)aSEnemyAI.target.position - rb.position).normalized;
        attackMove = true;
    }

    public void AttackAnimEndEvent() {
        attackMove = false;
    }

    private void AttackMove() {
        rb.MovePosition(rb.position + (attackDirection.normalized * speed * Time.fixedDeltaTime));
        enemyAnimController.SetAnimMoveDirection(attackDirection);
    }

    private void FixedUpdate() {
        if (attackMove) {
            AttackMove();
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (!attackMove) {return;}
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>(); 
        if(playerHealth) {
            if (!playerCollision) {
                playerCollision = true;
                playerHealth.TakeDamage(1, other.transform);
                StartCoroutine(CollisionReloadRoutine(playerHealth.damageRecoveryTime));
            }
            
        }

    }

    private IEnumerator CollisionReloadRoutine(float cooldown) {
        yield return new WaitForSeconds(cooldown);
        playerCollision = false;
    }
}
