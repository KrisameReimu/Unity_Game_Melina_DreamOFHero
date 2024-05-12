using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float cameraOffset = 2f; // Distance the camera moves up
    private bool isLookingUp = false; // Flag to indicate if the player is looking up
   
    void Awake()
    {
        // Find the player object by tag
        target = GameObject.FindWithTag("Player").transform;

        if (target == null)
        {
            Debug.LogError("Player object not found. Make sure the player object is tagged with 'Player'.");
        }
    }

    void LateUpdate()
    {
        // Check if the target object is not null before accessing its position
        if (target != null)
        {
            Vector3 desiredPosition = target.position;

            if (Input.GetKey(KeyCode.W))
            {
                isLookingUp = true;
                desiredPosition += Vector3.up * cameraOffset; // Move the camera up
            }
            else
            {
                isLookingUp = false;
            }

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y + 0.3f, transform.position.z);
        }
    }
}
