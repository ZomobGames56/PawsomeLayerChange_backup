using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public Transform center; // The point around which the AI will move
    public float radius = 5f; // Radius of the circular path
    public float speed = 2f; // Speed of movement

    private float angle = 0f; // Current angle in radians

    void Update()
    {
        // Calculate the new angle
        angle += speed * Time.deltaTime;

        // Calculate the new position based on the angle
        float x = center.position.x + Mathf.Cos(angle) * radius;
        float z = center.position.z + Mathf.Sin(angle) * radius;

        // Update the position of the AI
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
