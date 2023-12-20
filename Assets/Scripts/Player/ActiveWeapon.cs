// Active Weapon is a singleton that keeps track of the current weapon the player is using and calls the attack function on that weapon when the player presses the attack button. 
// It also keeps track of the player's input and calls the secondary attack function on the weapon when the player presses the secondary attack button. 
// It also has a function to reset the weapon related attributes, checks, and animator states to its default state.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon {get; private set;}
    private PlayerControls playerControls;

    public bool WeaponChanged = false;
    public bool disableAttack = false;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
        playerControls.Combat.SecondaryAttack.started += _ => SecondaryAttackStart();
        playerControls.Combat.SecondaryAttack.canceled += _ => SecondaryAttackStop();


    }

    private void Update() {
        
    }

    public void NewWeapon(MonoBehaviour newWeapon) {
        CurrentActiveWeapon = newWeapon;
        WeaponChanged = true;
    }

    public void WeaponNull() {
        CurrentActiveWeapon = null;
    }

    private void Attack() {
        if (disableAttack) {return;}
        if (CurrentActiveWeapon) {
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }

    private void SecondaryAttackStart() {
        if (disableAttack) {return;}
        if (CurrentActiveWeapon) {
            (CurrentActiveWeapon as IWeapon).SecondaryAttackStart();
        }
        
    }

    private void SecondaryAttackStop() {
        //if (disableAttack) {return;}
        if (CurrentActiveWeapon) {
            (CurrentActiveWeapon as IWeapon).SecondaryAttackStop();
        }
    }

    public void WeaponReset() {
        (CurrentActiveWeapon as IWeapon).WeaponReset();
    }

}
