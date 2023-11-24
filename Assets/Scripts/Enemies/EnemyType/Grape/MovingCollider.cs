using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCollider : MonoBehaviour
{
   private CircleCollider2D circleCollider;
    private float currentOffset = 0f;
    private float startingOffset = 0f;
    [SerializeField] private float maxOffset = 0.6f;
    [SerializeField] private float increment = 0.2f;
    [SerializeField] private float startDelay = 0.1f;
    [SerializeField] private float repeatDelay = 0.1f;


    void Start()
    {
        // Get the CircleCollider2D component on this GameObject
        circleCollider = GetComponent<CircleCollider2D>();

        if (circleCollider == null)
        {
            Debug.LogError("CircleCollider2D component not found on this GameObject.");
            enabled = false; // Disable the script if the CircleCollider2D is not found
        }

        startingOffset = circleCollider.offset.x;
        currentOffset = startingOffset;
        
    }

    public void ColliderStartMoving() {
        // Start the repeating offset change after startDelay seconds, and repeat every repeatDelay seconds
        InvokeRepeating("ChangeOffset", startDelay, repeatDelay);
        
    }

    // Method to change the circle collider's offset
    void ChangeOffset()
    {
        // Increment the current offset
        currentOffset += increment;
        
        // Clamp the offset to the maximum value
        currentOffset = Mathf.Clamp(currentOffset, 0f, maxOffset);

        // Set the offset to the CircleCollider2D component
        circleCollider.offset = new Vector2(currentOffset, circleCollider.offset.y);

        // If the maximum offset is reached, stop the repeating Invoke
        if (currentOffset >= maxOffset)
        {
            CancelInvoke("ChangeOffset");
            currentOffset = startingOffset;
            circleCollider.offset = new Vector2(currentOffset, circleCollider.offset.y);
            
        }
    }
}
