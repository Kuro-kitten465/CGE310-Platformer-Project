using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Assign the player GameObject
    public float fixedYPosition = 2f; // Set a fixed Y position
    public float smoothSpeed = 5f;  // Adjust for smoother movement

    private float centerX;  // Center of the screen in world units

    void Start()
    {
        // Set the initial camera position as the center reference
        centerX = transform.position.x;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Only move the camera if the player moves past the center
            if (player.position.x > centerX)
            {
                float targetX = player.position.x;
                Vector3 targetPosition = new Vector3(targetX, fixedYPosition, transform.position.z);

                // Smoothly interpolate the camera position
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            }
        }
    }
}
