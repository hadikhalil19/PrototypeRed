using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private int projectileDamage = 1;



    //private WeaponInfo weaponInfo;
    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
    }


    private void Update() {
        MoveProjectile();
        DetectFireDistance();
    }

    //public void UpdateWeaponInfo(WeaponInfo weaponInfo) {
    //    this.weaponInfo = weaponInfo;
   // }

    public void UpdateProjectileRange(float projectileRange){
        this.projectileRange = projectileRange;
    }
    public void UpdateProjectileDamage(int projectileDamage){
        this.projectileDamage = projectileDamage;
    }

    public void UpdateMoveSpeed(float bulletMoveSpeed){
        this.moveSpeed = bulletMoveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructibles indestructibles = other.gameObject.GetComponent<Indestructibles>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        if ((player && isEnemyProjectile)) {
                player?.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            } else if (enemyHealth && !isEnemyProjectile) {
                enemyHealth?.TakeDamage(projectileDamage);
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            } else if (!other.isTrigger && indestructibles) {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
    
    }

    private void DetectFireDistance() {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange) {
            Destroy(gameObject);
        }
    }


    private void MoveProjectile() {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
