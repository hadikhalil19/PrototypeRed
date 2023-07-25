using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool GettingKnockedBack {get; private set;}
    private Rigidbody2D myRigidBody;
    [SerializeField] float knockBackTime = 0.1f;


   private void Awake() {
    myRigidBody = GetComponent<Rigidbody2D>();

   }

    public void GetKnockedBack(Transform damageSource, float knockBackThurst) {
        GettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThurst * myRigidBody.mass;
        myRigidBody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockBackRoutine());
    }

    private IEnumerator KnockBackRoutine () {
        yield return new WaitForSeconds(knockBackTime);
        myRigidBody.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }

}
