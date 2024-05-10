using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float cameraOffset = 2f; // Distance the camera moves up
    private bool isLookingUp = false; // Flag to indicate if the player is looking up

    void LateUpdate()
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
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
