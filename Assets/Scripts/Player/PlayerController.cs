using System.Collections;
using System.Collections.Generic;
using Proto.Dialogue;
using UnityEngine;
using Proto.Saving;
using Proto.Audio;

public class PlayerController : Singleton<PlayerController>, ISaveable
{
    
    
    
    PlayerControls playerControls;
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] float attackMoveThurst = 10f; 
    [SerializeField] float attackMoveTime = 0.2f;
    [SerializeField] float sprintAttackThurst = 10f; 
    [SerializeField] float sprintAttackTime = 0.5f;
    [SerializeField] float sprintStopDelay = 0.2f;
    public Vector2 LastFacingDirection { get {return lastFacingDirection;}}
    public bool AttackMoving {get; set;}
    public bool AttackDirectionLock {get; set;}
    public bool SpriteFlipLock {get; set;}
    public bool MoveLock;
    public bool AttackLock;
    public Vector2 movement {get; set;}
    public Vector3 playerLookAt {get; set;}
    public bool sprint = false;
    public bool sprintAttack = false;
    Vector2 direction = new Vector2 (0,0);
    //public Vector2 PlayerDirection {get {return direction;}}
    private float mouseFollowDelay = 0.2f;
    private MousePosition mousePosition;
    private KnockBack knockBack;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private ActiveWeapon activeWeapon;
    private Vector2 lastFacingDirection; // added Vector2 variable to store the last facing direction
    private Vector2 lastMovement; // added Vector2 variable to store the last movement value when sprinting.
    //private SwordAttack swordAttack;
    private PlayerConversant playerConversant;
    private FootstepAudioPlayer footstepAudioPlayer;
    private bool sprintStop = false;
    private bool footstepLeft = false;
    private bool footstepRight = false;
    private bool attackMoveAudio = true;

    readonly int CHANGEWEAPON_HASH = Animator.StringToHash("ChangeWeapon");
    readonly int ISATTACKING_HASH = Animator.StringToHash("isAttacking");
    readonly int LIGHTSPEARATTACKING_HASH = Animator.StringToHash("LightSpearAttacking");
    readonly int BOWAIM_HASH = Animator.StringToHash("BowAim");
    readonly int BOWATTACK_HASH = Animator.StringToHash("BowAttack");
    readonly int SHOOTARROW_HASH = Animator.StringToHash("ShootArrow");
    readonly int NOCKINGARROW_HASH = Animator.StringToHash("NockingArrow");
    readonly int RELOADARROW_HASH = Animator.StringToHash("ReloadArrow");
    readonly int SPRINT_HASH = Animator.StringToHash("Sprint");
    readonly int SPRINTING_HASH = Animator.StringToHash("Sprinting");


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
        footstepAudioPlayer = GetComponentInChildren<FootstepAudioPlayer>();
    }

    private void Start() {
        ActiveInventory.Instance.EquipStartingWeapon();
        playerControls.Movement.Sprint.performed += _ => Sprint();
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
        if (PlayerHealth.Instance.IsDead) {return;}
        if (playerConversant.isTalking) { // if in dialogue disable active weapon attacks and secondary attacks 
            ActiveWeapon.Instance.disableAttack = true;
            ForcePlayerLookAt();
            return;
        } else {
            ActiveWeapon.Instance.disableAttack = false;
        }
        if (!AttackMoving) {  
            Move();
            TriggerFootstepAudio();          
            AdjustPlayerFacingDirection();
        } else if (sprintAttack) {
            attackMove(sprintAttackThurst, sprintAttackTime);
        } else {
            attackMove(attackMoveThurst, attackMoveTime);
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

    private void attackMove(float MoveThurst, float MoveTime) {
        //if (MoveLock == false) {return;} // movelock becomes true as soon as attacking so this checks if attacking and returns if not 
        Vector2 direction = playerControls.Movement.Move.ReadValue<Vector2>();
        Vector2 difference = direction.normalized * MoveThurst * myRigidBody.mass;
        myRigidBody.AddForce(difference, ForceMode2D.Impulse);
        if (attackMoveAudio) {
            footstepAudioPlayer.PlayFootstepAudioClip();
            attackMoveAudio = false;
        }
        StartCoroutine(AttackMoveRoutine(MoveTime));
    }
    private IEnumerator AttackMoveRoutine (float MoveTime) {
        yield return new WaitForSeconds(MoveTime);
        myRigidBody.velocity = Vector2.zero;
        AttackMoving = false;
        attackMoveAudio = true;
    }

    private void Move() {
        if (knockBack.GettingKnockedBack || PlayerHealth.Instance.IsDead) { return; }
        if (MoveLock) {return;}
        if (sprint) {
            myRigidBody.MovePosition(myRigidBody.position + (movement.normalized * sprintSpeed * Time.fixedDeltaTime));
        } else if (sprintStop) {
            myRigidBody.MovePosition(myRigidBody.position + (lastMovement.normalized * moveSpeed * Time.fixedDeltaTime));
        
        } else {
            myRigidBody.MovePosition(myRigidBody.position + (movement.normalized * moveSpeed * Time.fixedDeltaTime));
        
        }
        
    }

    private void Sprint() {
        if (movement.magnitude < 0.1f) { return;}
        if (MoveLock) {return;}
        if (sprint) {return;}
        sprint = true;
        myAnimator.SetBool(SPRINTING_HASH, true);
        myAnimator.SetTrigger(SPRINT_HASH);
    }

    private void AdjustPlayerFacingDirection() { // Update Player Anim method
        if (AttackDirectionLock == true) {return;} // Slash hitbox is active
        if (myAnimator.GetBool("isRolling")) {return;}
        if (sprintAttack) {return;}
        
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
            if (sprint) {
                SprintStop();  
            }
        } else {
            lastMovement = movement;
            if (movement.x < 0) {
                mySpriteRender.flipX = true;
            } else {
                mySpriteRender.flipX = false;
            }
        } 
    }


    private void SprintStop() {
        sprint = false;
        myAnimator.SetBool(SPRINTING_HASH, false);
        sprintStop = true;
        StartCoroutine(StopSprintRoutine());
    }

    private IEnumerator StopSprintRoutine() {
        yield return new WaitForSeconds(sprintStopDelay);
        sprintStop = false;
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

    //  private IEnumerator MouseDirectionWithDelay() {
    //     Vector3 mousePos = Input.mousePosition;
    //     Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
    //     yield return new WaitForSeconds(mouseFollowDelay);
    //     direction = (mousePos - playerScreenPoint).normalized;
    //  }

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

    private void ForcePlayerLookAt() {
        Vector2 lookAtDirection = (playerLookAt - transform.position).normalized;
        myAnimator.SetFloat("idleX", lookAtDirection.x);
        myAnimator.SetFloat("idleY", lookAtDirection.y);
        if (playerLookAt.x > transform.position.x) {
            mySpriteRender.flipX = false;
        } else {
            mySpriteRender.flipX = true;
        }

    }

    public object CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        SerializableVector3 position = (SerializableVector3)state;
        transform.position = position.ToVector();
            
    }

    public void FootstepLeftAnimEvent() {
        footstepLeft = true;
    }

    public void FootstepRightAnimEvent() {
        footstepRight = true;
    }

    private void TriggerFootstepAudio() {
        if (footstepLeft) {
            footstepAudioPlayer.PlayFootstepAudioClip();
            footstepLeft = false;
        } else if (footstepRight) {
            footstepAudioPlayer.PlayFootstepAudioClip();
            footstepRight = false;
        }

        
    }

    public void ResetPlayerController() {
        MoveLock = false;
        AttackDirectionLock = false;
        AttackLock = false;
        sprint = false;
        sprintAttack = false;
        SpriteFlipLock = false;
    }
    
}

