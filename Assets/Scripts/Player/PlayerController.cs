using System.Collections;
using System.Collections.Generic;
using Proto.Dialogue;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    
    
    
    PlayerControls playerControls;
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] float attackMoveThurst = 10f; 
    [SerializeField] float attackMoveTime = 0.2f;
    public Vector2 LastFacingDirection { get {return lastFacingDirection;}}
    public bool AttackMoving {get; set;}
    public bool AttackDirectionLock {get; set;}
    public bool SpriteFlipLock {get; set;}
    public bool MoveLock {get; set;}
    public bool AttackLock {get; set;}
    public Vector2 movement {get; set;}

    Vector2 direction = new Vector2 (0,0);
    private float mouseFollowDelay = 0.2f;
    private MousePosition mousePosition;
    private KnockBack knockBack;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private ActiveWeapon activeWeapon;
    private Vector2 lastFacingDirection; // added Vector2 variable to store the last facing direction
    //private SwordAttack swordAttack;
    private PlayerConversant playerConversant;

    readonly int CHANGEWEAPON_HASH = Animator.StringToHash("ChangeWeapon");
    readonly int ISATTACKING_HASH = Animator.StringToHash("isAttacking");
    readonly int LIGHTSPEARATTACKING_HASH = Animator.StringToHash("LightSpearAttacking");
    readonly int BOWAIM_HASH = Animator.StringToHash("BowAim");
    readonly int BOWATTACK_HASH = Animator.StringToHash("BowAttack");
    readonly int SHOOTARROW_HASH = Animator.StringToHash("ShootArrow");
    readonly int NOCKINGARROW_HASH = Animator.StringToHash("NockingArrow");
    readonly int RELOADARROW_HASH = Animator.StringToHash("ReloadArrow");



    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        //swordAttack = GetComponentInChildren<SwordAttack>();
        mousePosition = GetComponent<MousePosition>();
        knockBack = GetComponent<KnockBack>();
        activeWeapon = GetComponentInChildren<ActiveWeapon>();
        playerConversant = GetComponent<PlayerConversant>();

    }

    private void Start() {
        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Update() {
        PlayerInput();
    }
    
    private void FixedUpdate() {
        if (playerConversant.isTalking) { // if in dialogue disable active weapon attacks and secondary attacks 
            ActiveWeapon.Instance.disableAttack = true;
            return;
        } else {
            ActiveWeapon.Instance.disableAttack = false;
        }
        if (!AttackMoving) {  
            Move();          
            AdjustPlayerFacingDirection();
        } else {
            attackMove();
        }
        //WeaponSwap();
    }

    private void PlayerInput() {
        if (MoveLock == false && !playerConversant.isTalking) { // if not movelock and not currently in a dialogue
            movement = playerControls.Movement.Move.ReadValue<Vector2>();
        } else {
            movement = new Vector2 (0,0);
        }
        

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
        myAnimator.SetFloat("speed", movement.sqrMagnitude);
        
    }

    private void attackMove() {
        if (MoveLock == false) {return;} // movelock becomes true as soon as attacking so this checks if attacking and returns if not 
        Vector2 direction = playerControls.Movement.Move.ReadValue<Vector2>();
        Vector2 difference = direction.normalized * attackMoveThurst * myRigidBody.mass;
        myRigidBody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(AttackMoveRoutine());
    }
    private IEnumerator AttackMoveRoutine () {
        yield return new WaitForSeconds(attackMoveTime);
        myRigidBody.velocity = Vector2.zero;
        AttackMoving = false;
    }

    private void Move() {
        if (knockBack.GettingKnockedBack || PlayerHealth.Instance.IsDead) { return; }
        if (MoveLock) {return;}
        myRigidBody.MovePosition(myRigidBody.position + (movement.normalized * moveSpeed * Time.fixedDeltaTime));
        
    }

    private void AdjustPlayerFacingDirection() {
        if (AttackDirectionLock == true) {return;} // Slash hitbox is active
        if (myAnimator.GetBool("isRolling")) {return;}
        
        //StartCoroutine(mouseDirectionWithDelay());
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        direction = (mousePos - playerScreenPoint).normalized;
        //direction = mousePosition.MouseDirectionSampled;
        if (direction.magnitude > 0.15f) // if the player is moving towards the mouse position
        {
            lastFacingDirection = direction; // set the last facing direction to the normalized direction
        }
        if (movement.magnitude < 0.1f) {
            myAnimator.SetFloat("idleX", lastFacingDirection.x);
            myAnimator.SetFloat("idleY", lastFacingDirection.y);
            AdjustIdleAnimDirection();
            } else {
                if (movement.x < 0) {
                    mySpriteRender.flipX = true;
                } else {
                    mySpriteRender.flipX = false;
                }
            } 
    }

    private void AdjustIdleAnimDirection() {
        if (SpriteFlipLock == true) {return;}
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;
        
        if (angle >= 112.5 || angle <= -112.5) {
            mySpriteRender.flipX = true;
        } else {
            mySpriteRender.flipX = false;
        }

    }

     private IEnumerator mouseDirectionWithDelay() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        yield return new WaitForSeconds(mouseFollowDelay);
        direction = (mousePos - playerScreenPoint).normalized;
     }

     private void WeaponSwap() {
        if (activeWeapon.WeaponChanged) {
            activeWeapon.WeaponChanged = false;
            myAnimator.SetTrigger(CHANGEWEAPON_HASH);
            myAnimator.SetBool(ISATTACKING_HASH, false);
            myAnimator.SetBool(LIGHTSPEARATTACKING_HASH, false);
            myAnimator.SetBool(BOWAIM_HASH, false);
            myAnimator.SetBool(BOWATTACK_HASH, false);
            myAnimator.SetBool(SHOOTARROW_HASH, false);
            myAnimator.SetBool(NOCKINGARROW_HASH, false);
            myAnimator.SetBool(RELOADARROW_HASH, false);
        }
     }

}

