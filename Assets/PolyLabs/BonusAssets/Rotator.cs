using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
    public Vector3 rotationAmount; 
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 
    }

    void FixedUpdate()
    {
        if (!HY_StartPause.countOver) return;

        Quaternion deltaRot =
            Quaternion.Euler(rotationAmount * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * deltaRot);
    }
}
