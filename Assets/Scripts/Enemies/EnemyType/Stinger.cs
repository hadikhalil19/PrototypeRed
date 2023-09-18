using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour, IEnemy
{

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimController enemyAnimController;
    private ASEnemyAI aSEnemyAI;
    Rigidbody2D rb;

    [SerializeField] float speed = 200f;

    private bool attackMove = false;

    private Vector2 attackDirection;


    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
        rb = GetComponent<Rigidbody2D>();
        aSEnemyAI = GetComponent<ASEnemyAI>();
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

    public void AttackAnimEvent() {
        attackDirection = ((Vector2)aSEnemyAI.target.position - rb.position).normalized;
        attackMove = true;
    }

    private void AttackMove() {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) 
        {
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

    private void OnCollisionStay2D(Collision2D other) {
        if (!attackMove) {return;}
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>(); // only works for older enemyAI with no A*
        if(playerHealth) {
            
            playerHealth.TakeDamage(1, other.transform);
            
        }

    }

}
