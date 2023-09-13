using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarEnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    Vector2 movement;
    Vector2 lastFacingDirection;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private KnockBack knockBack;
    //private SpriteRenderer spriteRenderer;
    private EnemyAnimController enemyAnimController;
    
   private void Awake() {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimController =  GetComponent<EnemyAnimController>();
    }



    private void FixedUpdate() {
        if (!knockBack.GettingKnockedBack) {
            if (!enemyAnimController.isAttacking) {
                Move();
            }
        }
       
    }

    private void Move() {
        myRigidBody.MovePosition(myRigidBody.position + (movement.normalized * moveSpeed * Time.fixedDeltaTime));
        enemyAnimController.SetAnimMoveDirection(movement);
    }

    public void MoveTo(Vector2 targetPosition) {
        movement = targetPosition;

    }

    public void StopMoving () {
        movement = Vector3.zero;
    }
}
