using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
    public Vector3 rotationAmount; // degrees per second
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // important for rotating objects
    }

    void FixedUpdate()
    {
        Quaternion deltaRot =
            Quaternion.Euler(rotationAmount * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * deltaRot);
    }
}
