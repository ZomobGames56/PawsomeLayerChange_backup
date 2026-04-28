using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotatingLog : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.forward; // X, Y, or Z
    public float rotationSpeed = 100f; // degrees per second

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // IMPORTANT
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(rotationAxis * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}