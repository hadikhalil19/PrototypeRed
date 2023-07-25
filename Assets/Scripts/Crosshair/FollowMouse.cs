using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
   private void FixedUpdate()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = PlayerController.Instance.transform.position.z; // Ensure the same z-position as the player

        // Move the game object to the mouse position
        transform.position = worldPosition;
    }
}
