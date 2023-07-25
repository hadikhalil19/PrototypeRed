using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPlayer : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 720f; // Rotation speed in degrees per second
    [SerializeField] private float radius = 2f; // Radius of rotation
    //[SerializeField] Transform targetTransform;

    private Vector3 zAxis = new Vector3(0, 0, 1); // Z-axis vector for rotation

    private void Awake() {
    }
    private void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = PlayerController.Instance.transform.position.z; // Ensure the same z-position as the player

        // Calculate the direction from the player to the mouse position
        Vector3 direction = mousePos - PlayerController.Instance.transform.position;

        // Calculate the target position around the player based on the radius and direction
        Vector3 targetPosition = PlayerController.Instance.transform.position + direction.normalized * radius;
    
        // Set the object's position to the target position
        transform.position = targetPosition;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        targetRotation *= Quaternion.Euler(0, 0, 90f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        
        
        //targetTransform.position =  mousePos;


        
        //transform.LookAt(targetTransform, zAxis);

        //transform.right = targetTransform.position - transform.position;
    }


}

