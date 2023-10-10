using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefab;

    [SerializeField] private Transform MeleeAttackCollider;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimController enemyAnimController;
    private bool meleeAttack = false;


    //readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
    }

    public void Attack() { 
        //needs to have stop while attacking
        meleeAttack = false;
        TriggerAttackAnim();
    }

    private void TriggerAttackAnim() {
        if (!meleeAttack) {
            enemyAnimController?.PlayAttackAnim();
        } else {
            enemyAnimController?.PlaySecondaryAnim();
        }
        
        if (transform.position.x - PlayerController.Instance.transform.position.x < 0) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }

        myAnimator.SetFloat("idleX", transform.position.x - PlayerController.Instance.transform.position.x );
        myAnimator.SetFloat("idleY", PlayerController.Instance.transform.position.y - transform.position.y);
    }

    public void SecondaryAttack() {
        meleeAttack = true;
        TriggerAttackAnim();
    }

    public void SpawnProjectileAnimEvent() {
        GameObject newBullet = Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
        newBullet.transform.right = PlayerController.Instance.transform.position - newBullet.transform.position;
    }

    public void SecondaryAttackAnimEvent() {
        Debug.Log("melee attack");

        Vector3 myPos = this.transform.position;
        Vector3 playerPos = PlayerController.Instance.transform.position;
        float angle = Mathf.Atan2(playerPos.y - myPos.y, playerPos.x -  myPos.x) * Mathf.Rad2Deg;
        if (angle > 67.5 && angle <= 112.5) { // north
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (angle > 22.5 && angle <= 67.5) { // northeast
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 0, 45);
        } else if (angle > -22.5 && angle <= 22.5) { // east
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (angle > -67.5 && angle <= -22.5) { // southeast
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 0, -45);
        } else if (angle > -112.5 && angle <= -67.5) { // south
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (angle > -157.5 && angle <= -112.5) { // southwest
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 0, -135);
        } else if (angle > 112.5 && angle <= 157.5) { // northwest
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 0, 135);
        } else if (angle > 157.5 || angle < -157.5 ) { // west
            MeleeAttackCollider.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        MeleeAttackCollider.gameObject.SetActive(true);
        StartCoroutine(MeleeColliderDisableRoutine());
    }

    private IEnumerator MeleeColliderDisableRoutine() {
        yield return new WaitForSeconds(0.1f);
        MeleeAttackCollider.gameObject.SetActive(false);
    }

}
