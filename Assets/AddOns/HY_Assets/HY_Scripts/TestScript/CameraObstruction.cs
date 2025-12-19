using UnityEngine;

public class CameraObstruction : MonoBehaviour
{
    public Transform player;  // The player or the object to focus on
    public float smoothSpeed = 0.2f;  // Speed for smooth camera movement
    public float cameraDistance = 5.0f;  // Default camera distance
    private Vector3 offset;
    private Camera mainCamera;

    void Start()
    {
        // Initial offset between camera and player
        offset = transform.position - player.position;
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Cast a ray from the camera towards the player
        RaycastHit hit;
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(player.position, transform.position);

        // Check if something is obstructing the camera's view
        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            // If the ray hits an object that is not the player, we have an obstruction
            if (hit.collider.gameObject != player.gameObject)
            {
                // Move the camera closer to the player to avoid the obstruction
                transform.position = Vector3.Lerp(transform.position, hit.point, smoothSpeed);
            }
        }
        else
        {
            // No obstruction, return to the original offset position
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }

        // Look at the player
        transform.LookAt(player);
    }
}
