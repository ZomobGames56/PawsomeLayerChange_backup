using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float enginePower = 1500f;   // Forward thrust
    [SerializeField] private float reversePower = 800f;   // Reverse thrust
    [SerializeField] private float turnSpeed = 50f;       // Degrees per second
    [SerializeField] private float maxSpeed = 15f;        // Max forward speed

    [Header("Water Physics")]
    [SerializeField] private float waterDrag = 0.5f;      // Linear drag
    [SerializeField] private float waterAngularDrag = 2f; // Angular drag
    [SerializeField] private float stabilityFactor = 0.5f; // Anti-roll torque

    private Rigidbody rb;
    private float throttle; // W/S input
    private float steer;    // A/D input

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = waterDrag;
        rb.angularDamping = waterAngularDrag;
        rb.centerOfMass = Vector3.down * 0.5f; // Lower center for stability

        // Optional: freeze X/Z rotation to reduce tipping
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Get player input
        throttle = Input.GetAxis("Vertical");   // W/S or Up/Down arrows
        steer = Input.GetAxis("Horizontal");    // A/D or Left/Right arrows
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        ApplyWaterDrag();
        ApplyStability();
        ClampMaxSpeed();
    }

    void HandleMovement()
    {
        // Forward/backward force
        if (throttle > 0)
        {
            rb.AddForce(transform.forward * throttle * enginePower);
        }
        else if (throttle < 0)
        {
            rb.AddForce(transform.forward * throttle * reversePower);
        }
    }

    void HandleSteering()
    {
        // Rotate the boat around Y-axis for responsive turning
        if (Mathf.Abs(throttle) > 0.1f || rb.linearVelocity.magnitude > 0.5f)
        {
            float turn = steer * turnSpeed * Time.fixedDeltaTime;
            transform.Rotate(0f, turn, 0f, Space.World);
        }
    }

    void ApplyWaterDrag()
    {
        // Apply linear drag proportional to horizontal velocity
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(-horizontalVel * waterDrag);
    }

    void ApplyStability()
    {
        // Simple anti-roll to keep boat upright
        Vector3 uprightTorque = Vector3.Cross(transform.up, Vector3.up) * stabilityFactor * 100f;
        rb.AddTorque(uprightTorque);
    }

    void ClampMaxSpeed()
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            Vector3 clampedVel = horizontalVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(clampedVel.x, rb.linearVelocity.y, clampedVel.z);
        }
    }
}
