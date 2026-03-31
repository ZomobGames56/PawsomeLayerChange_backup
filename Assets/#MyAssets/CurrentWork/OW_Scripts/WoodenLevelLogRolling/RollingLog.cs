using UnityEngine;

public class RollingLog : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 120f;
    [SerializeField] Vector3 axis = Vector3.forward;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MoveRotation(
            rb.rotation * Quaternion.AngleAxis(rotationSpeed * Time.fixedDeltaTime, axis)
        );
    }

    public Vector3 GetSurfaceVelocity(Vector3 point)
    {
        Vector3 centerToPoint = point - rb.worldCenterOfMass;
        return Vector3.Cross(axis.normalized * Mathf.Deg2Rad * rotationSpeed, centerToPoint);
    }
}
