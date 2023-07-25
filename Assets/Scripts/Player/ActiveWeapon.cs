using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon {get; private set;}
    private PlayerControls playerControls;

    public bool WeaponChanged = false;

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
        if (CurrentActiveWeapon) {
            (CurrentActiveWeapon as IWeapon).Attack();
        }
        
    }

    private void SecondaryAttackStart() {
        if (CurrentActiveWeapon) {
            (CurrentActiveWeapon as IWeapon).SecondaryAttackStart();
        }
        
    }

    private void SecondaryAttackStop() {
        if (CurrentActiveWeapon) {
            (CurrentActiveWeapon as IWeapon).SecondaryAttackStop();
        }
        
    }

}
