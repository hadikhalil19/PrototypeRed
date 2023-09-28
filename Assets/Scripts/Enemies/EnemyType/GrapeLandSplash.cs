using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeLandSplash : MonoBehaviour
{
    private SpriteFade spriteFade;
    [SerializeField] private float splashDamageDuration = 1f;
    private void Awake() {
        spriteFade = GetComponent<SpriteFade>();
    }

    void Start()
    {  
        StartCoroutine(spriteFade.SlowFadeRoutine());
        Invoke("DisableCollider", splashDamageDuration);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(1, transform);
    }

    private void DisableCollider() {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

}
