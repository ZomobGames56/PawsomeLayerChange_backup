using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // This method is called when the bomb is reactivated
    void OnEnable()
    {
        rb.linearVelocity = Vector3.zero; // Reset the velocity
        rb.angularVelocity = Vector3.zero; // Reset angular velocity
    }
}
