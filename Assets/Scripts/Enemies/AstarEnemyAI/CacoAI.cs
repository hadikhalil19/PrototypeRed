using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor.Tilemaps;

public class CacoAI : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float attackRange = 3f;

    [SerializeField] private float followRange = 10f;

    [SerializeField] private float meleeRange = 1f;

    [SerializeField] float roamChangeDirTime = 2f;
    [SerializeField] bool hasCollisonDamage = false; 
    public float nextWaypointDistance = 1f;
    
    private Path path;
    private int currentWaypoint = 0;

    bool reachedEndOfPath = false;
    
    Seeker seeker;
    Rigidbody2D rb;
    
    public float pathUpdateSeconds = 0.5f;

    private enum State{
        Roaming,
        Attacking,
        MeleeAttack,
        Following,
        Flanking
    }

    private State state;

    private AstarEnemyPathfinding enemyPathfinding;

    private float timeRoaming = 0f;
    private Vector2 roamPosition;

    [SerializeField] MonoBehaviour enemyType;
    [SerializeField] private float attackCooldownMin = 2f;
    [SerializeField] private float attckRandMaxCD = 3f;
    [SerializeField] private float meleeCooldownMin = 1f;
    [SerializeField] private float meleeRandMaxCD = 3f;
    [SerializeField] private float looseInterestTime = 2f;
    [SerializeField] private float flankingTimeMin = 1.5f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;
    private bool canMeleeAttack = true;
   
    private bool loosingInterest = false;

    private bool clockwise = false;

    public bool StartFlanking = false;

    private EnemyHealth enemyHealth;
    //private int meleeAttackStage = 0;
    private BossUI bossUI;
    private bool BossHealthVisible = false;

    private void Awake() {
        state = State.Roaming;
        enemyPathfinding = GetComponent<AstarEnemyPathfinding>();
        enemyHealth = GetComponent<EnemyHealth>();    
        bossUI = GetComponent<BossUI>();
    }

    public void Start()
    {
        target = PlayerController.Instance.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void Update() {
        MovementStateControl();
    }

    private void MovementStateControl() {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                
            break;
            
            case State.Attacking:
                Attacking();
                
            break;

            case State.MeleeAttack:
                MeleeAttack();
                
            break;

            case State.Following:
                Following();
                
            break;

            case State.Flanking:
                Flanking();
                
            break;
        }
    }

    private void Roaming() {

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange && canAttack) {
            state = State.Attacking;
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < followRange) {
            state = State.Following;
            return;
        }
        
        timeRoaming += Time.deltaTime;
        
        enemyPathfinding.MoveTo(roamPosition);

        if (timeRoaming > roamChangeDirTime) {
            roamPosition = GetRoamingPosition();
        }
 
    }

    private void Attacking() {
        if (enemyHealth.dying) {return;}
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > followRange) {
            if (!loosingInterest) {
                loosingInterest = true;
                StartCoroutine(RoamAgainRoutine());
            }
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange) {
            state = State.Following;
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < meleeRange) {
            state = State.MeleeAttack;
        }

        if (canAttack && attackRange != 0) {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking) {
                enemyPathfinding.StopMoving();
            } else {
                enemyPathfinding.MoveTo(GetRoamingPosition());
            }

            StartCoroutine(AttackCooldownRoutine());
        } else {
            state = State.Following;
        }
    }

    private IEnumerator AttackCooldownRoutine() {
        float cooldown =  attackCooldownMin + Random.Range(0,3f);
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    private IEnumerator MeleeCooldownRoutine() {
        float cooldown =  meleeCooldownMin + Random.Range(0,meleeRandMaxCD);
        yield return new WaitForSeconds(cooldown);
        canMeleeAttack = true;
    }

    private void MeleeAttack() {
        if (enemyHealth.dying) {return;}
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > meleeRange && canAttack) {
            state = State.Attacking;
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange) {
            state = State.Following;
        }

        if (canMeleeAttack && attackRange != 0) {
            canMeleeAttack = false;
            (enemyType as IEnemy).SecondaryAttack();

            if (stopMovingWhileAttacking) {
                enemyPathfinding.StopMoving();
            // } else {
            //     enemyPathfinding.MoveTo(GetRoamingPosition());
            }

            StartCoroutine(MeleeCooldownRoutine());
        } else {
            state = State.Following;
        }

    }


    private void Following() {
        // if BossUI health is not visible, start it up when the boss first starts the following state.
        if (!BossHealthVisible) { 
            BossHealthVisible = true;
            bossUI.BossHealthStartUp();
        }
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > followRange) {
            if (!loosingInterest) {
                loosingInterest = true;
                StartCoroutine(RoamAgainRoutine());
            }
            
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < meleeRange && canMeleeAttack) {
            state = State.MeleeAttack;
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange && canAttack && canMeleeAttack) {
            state = State.Attacking;
        }
        
        if (target == null) {
            target = PlayerController.Instance.transform;
        }
        if (StartFlanking) {
            state = State.Flanking;
        } else {
            FollowTarget();
        }
        
    }

    private IEnumerator RoamAgainRoutine() {
        yield return new WaitForSeconds(looseInterestTime);
        state = State.Roaming;
        loosingInterest = false;
    }

    void UpdatePath() {

        if(seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FollowTarget() {
        if (path == null) {return;}

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;

        //rb.AddForce(force);
        enemyPathfinding.MoveTo(direction);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f)).normalized;
    
    }

    void diagonalMove() {
        Vector2 direction = (Vector2)target.position - rb.position;
        Vector2 perpendicularVector = new Vector2(-direction.y, direction.x);
        if (!clockwise) {
            perpendicularVector = -perpendicularVector;
        }
        enemyPathfinding.MoveTo(perpendicularVector);
    }

    private void Flanking() {
        diagonalMove();
        if (StartFlanking) {
            StartFlanking = false;
            StartCoroutine(FlankStopRoutine());
        }
    }

    private IEnumerator FlankStopRoutine() {
        yield return new WaitForSeconds(flankingTimeMin + Random.Range(0,2f));
        state = State.Following;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            int randomNumber = Random.Range(1,4);
            if (randomNumber == 1) {
                StartFlanking = true;
                clockwise = true;
            } else if (randomNumber == 2) {
                StartFlanking = true;
                clockwise = false;
            } else if (randomNumber == 3) {
                StartFlanking = false;
            }
            
        }
    }
    
}
