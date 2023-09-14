using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarEnemyAI : MonoBehaviour
{
    [SerializeField] private float attackRange = 0f;
    
    public AIDestinationSetter aIDestinationSetter;

    private enum State{
        Roaming,
        Attacking
    }

    private State state;
   
   

    private void Awake() {
        state = State.Roaming;    
    }

    private void Start() {
        
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
        }
    }

    private void Roaming() {
       
        state = State.Attacking;
        aIDestinationSetter.target =  null;

    }

    private void Attacking() {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange) {
            state = State.Roaming;
        }
        
        aIDestinationSetter.target =  PlayerController.Instance.transform;
    }


}
