using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarTest : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float attackRange = 3f;

    [SerializeField] private float followRange = 10f;

     [SerializeField] float roamChangeDirTime = 2f; 
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
        Following
    }

    private State state;

    private AstarEnemyPathfinding enemyPathfinding;

    private float timeRoaming = 0f;
    private Vector2 roamPosition;

    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;
   
   

    private void Awake() {
        state = State.Roaming;
        enemyPathfinding = GetComponent<AstarEnemyPathfinding>();    
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
                Debug.Log("Roaming");
            break;
            
            case State.Attacking:
                Attacking();
                Debug.Log("Attacking");
            break;

            case State.Following:
                Following();
                Debug.Log("Attacking");
            break;
        }
    }

    private void Roaming() {
       
        timeRoaming += Time.deltaTime;
        
        
        enemyPathfinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange) {
            state = State.Attacking;
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < followRange) {
            state = State.Following;
        }


        if (timeRoaming > roamChangeDirTime) {
            roamPosition = GetRoamingPosition();
        }
 

    }

    private void Attacking() {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > followRange) {
            state = State.Roaming;
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange) {
            state = State.Following;
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
        }
    }

    private IEnumerator AttackCooldownRoutine() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    private void Following() {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > followRange) {
            state = State.Roaming;
        } else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange) {
            state = State.Attacking;
        }
        
        target = PlayerController.Instance.transform;
        FollowTarget();
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

    
}
