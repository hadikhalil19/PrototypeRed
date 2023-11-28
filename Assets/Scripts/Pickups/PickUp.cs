using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        ManaGlobe,
        HealthGlobe,
    }

    [SerializeField] private PickUpType pickUpType;

    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelartionRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;

    [SerializeField] private int manaGlobeValue = 25;
    [SerializeField] private int healthGlobeValue = 25;

    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update() {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance) {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelartionRate;
        } else {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate() {
        rb.velocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine() {
        Vector3 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-1f, 1f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);
        Vector2 endPoint = new Vector2(randomX, randomY);
        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);

            yield return null;
        }
    }

    private void DetectPickupType() {
        switch (pickUpType)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.IncrementCurrentGold();
                
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer(healthGlobeValue);
                
                break;
            case PickUpType.ManaGlobe:
                PlayerMana.Instance.ManaGlobeRefresh(manaGlobeValue);
                break;
            default:
                break;
        }
    }


}
