using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HybridPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float airControl = 0.5f;
    public float movementSharpness = 15f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float gravityMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 inputDir;

    // Platform tracking
    private Rigidbody currentPlatformRb;
    private Vector3 lastPlatformPos;
    private Quaternion lastPlatformRot;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        inputDir = new Vector3(h, 0, v).normalized;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        GroundCheck();

        Vector3 finalVelocity = CalculateMovement();

        // ADD platform motion (not override)
        finalVelocity += GetPlatformVelocity();

        // Apply velocity
        rb.linearVelocity = new Vector3(finalVelocity.x, rb.linearVelocity.y, finalVelocity.z);

        ApplyExtraGravity();
    }

    // ------------------ MOVEMENT ------------------
    Vector3 CalculateMovement()
    {
        Vector3 targetMove = inputDir * moveSpeed;

        if (!isGrounded)
            targetMove *= airControl;

        Vector3 current = rb.linearVelocity;

        // Smooth control
        current.x = Mathf.Lerp(current.x, targetMove.x, movementSharpness * Time.fixedDeltaTime);
        current.z = Mathf.Lerp(current.z, targetMove.z, movementSharpness * Time.fixedDeltaTime);

        return current;
    }

    // ------------------ PLATFORM VELOCITY ------------------
    Vector3 GetPlatformVelocity()
    {
        if (currentPlatformRb == null) return Vector3.zero;

        Vector3 deltaPos = currentPlatformRb.position - lastPlatformPos;

        Quaternion deltaRot = currentPlatformRb.rotation * Quaternion.Inverse(lastPlatformRot);

        Vector3 relativePos = rb.position - currentPlatformRb.position;
        Vector3 rotatedPos = deltaRot * relativePos;

        Vector3 rotationMove = rotatedPos - relativePos;

        lastPlatformPos = currentPlatformRb.position;
        lastPlatformRot = currentPlatformRb.rotation;

        return (deltaPos + rotationMove) / Time.fixedDeltaTime;
    }

    // ------------------ JUMP ------------------
    void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0;
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    // ------------------ GRAVITY ------------------
    void ApplyExtraGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // ------------------ GROUND ------------------
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
    }

    // ------------------ PLATFORM DETECTION ------------------
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Log"))
        {
            if (currentPlatformRb != collision.rigidbody)
            {
                currentPlatformRb = collision.rigidbody;
                lastPlatformPos = currentPlatformRb.position;
                lastPlatformRot = currentPlatformRb.rotation;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Log"))
        {
            currentPlatformRb = null;
        }
    }

    // ------------------ DEBUG ------------------
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}