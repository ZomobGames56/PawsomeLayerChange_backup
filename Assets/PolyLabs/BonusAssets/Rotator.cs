using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Quaternion delta = Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * delta);
    }
}
