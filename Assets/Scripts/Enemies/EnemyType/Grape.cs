using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefab;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimController enemyAnimController;


    //readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
    }

    public void Attack() { 
        //needs to have stop while attacking
        //myAnimator.SetTrigger(ATTACK_HASH);
        enemyAnimController?.PlayAttackAnim();
        if (transform.position.x - PlayerController.Instance.transform.position.x < 0) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }

        myAnimator.SetFloat("idleX", transform.position.x - PlayerController.Instance.transform.position.x );
        myAnimator.SetFloat("idleY", PlayerController.Instance.transform.position.y - transform.position.y);
        
    }

    public void SpawnProjectileAnimEvent() {
        //Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
        GameObject newBullet = Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
        newBullet.transform.right = PlayerController.Instance.transform.position - newBullet.transform.position;
    }

}
