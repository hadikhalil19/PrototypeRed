using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proto.Audio;

public class Stinger : MonoBehaviour, IEnemy
{

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimController enemyAnimController;
    private ASEnemyAI aSEnemyAI;
    private EnemyHealth myHealth;
    Rigidbody2D rb;

    [SerializeField] float speed = 200f;
    [SerializeField] int meleeDamage = 1;

    private bool attackMove = false;

    private Vector2 attackDirection;
    private GenericAudioPlayer genericAudioPlayer;
    private bool playerCollision = false;


    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
        rb = GetComponent<Rigidbody2D>();
        aSEnemyAI = GetComponent<ASEnemyAI>();
        genericAudioPlayer = GetComponentInChildren<GenericAudioPlayer>();
        myHealth = GetComponent<EnemyHealth>();
    }

    public void Attack() {
        enemyAnimController?.PlayAttackAnim();
        if (transform.position.x - PlayerController.Instance.transform.position.x < 0) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }

        myAnimator.SetFloat("idleX", transform.position.x - PlayerController.Instance.transform.position.x );
        myAnimator.SetFloat("idleY", PlayerController.Instance.transform.position.y - transform.position.y);
        
    }

    public void SecondaryAttack() {
    
    }

    public void AttackAnimEvent() {
        attackDirection = ((Vector2)aSEnemyAI.target.position - rb.position).normalized;
        attackMove = true;
        genericAudioPlayer.PlaySecondaryAudio();
    }

    private void AttackMove() {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            attackMove = true;
        } else {
            attackMove = false;
        }
        //Vector2 direction = ((Vector2)aSEnemyAI.target.position - rb.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;
        //rb.AddForce(force);

        rb.MovePosition(rb.position + (attackDirection.normalized * speed * Time.fixedDeltaTime));
        enemyAnimController.SetAnimMoveDirection(attackDirection);
        
    }

    private void FixedUpdate() {
        if (attackMove) {
            AttackMove();
        }
    }

    // private void OnCollisionStay2D(Collision2D other) {
    //     if (!attackMove) {return;}
    //     PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
    //     ShieldBlock shieldBlock = other.gameObject.GetComponent<ShieldBlock>(); 
    //     if (shieldBlock) {
    //         if (!playerCollision) {
    //             playerCollision = true;
    //             shieldBlock.TakeDamage(1, transform);
    //             StartCoroutine(CollisionReloadRoutine(playerHealth.damageRecoveryTime));
    //         }
    //     } else if (playerHealth) {
    //         if (!playerCollision) {
    //             playerCollision = true;
    //             playerHealth.TakeDamage(1, transform);
    //             StartCoroutine(CollisionReloadRoutine(playerHealth.damageRecoveryTime));
    //         }
    //     }

    // }

    private void OnCollisionStay2D(Collision2D other) {
        if (!attackMove) {return;}
        if (playerCollision) {return;}

        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        ShieldBlock shieldBlock = other.gameObject.GetComponentInChildren<ShieldBlock>();

        if (myHealth.attacksBlocked) {
            if (shieldBlock) {
                playerCollision = true;
                shieldBlock.TakeDamage(meleeDamage, transform);
                Debug.Log("shield block");
                StartCoroutine(CollisionReloadRoutine(0.3f));
            }     
        } else if(playerHealth) {
            playerCollision = true;
            playerHealth.TakeDamage(meleeDamage, transform);    
            StartCoroutine(CollisionReloadRoutine(playerHealth.damageRecoveryTime));
                
            
        }

    }

    private IEnumerator CollisionReloadRoutine(float cooldown) {
        yield return new WaitForSeconds(cooldown);
        playerCollision = false;
    }

}
