using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour, IWeapon
{
   [SerializeField] private WeaponInfo weaponInfo;

   [SerializeField] int healAmount = 30;

    public void Attack(){
        PlayerHealth.Instance.HealPlayer(healAmount);
    }
    public void SecondaryAttackStart(){}
    public void SecondaryAttackStop(){}
    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;

    }
    public void WeaponReset(){}
}
