using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded;
    Animator animator;
    void Start()
    {
        if (cam == null) cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent tipping over
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Camera relative input
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        moveInput = (camRight * h + camForward * v).normalized;

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = flatVel.magnitude;
        animator.SetFloat("Run", speed); // 👈 Blend Tree parameter
    }

    void FixedUpdate()
    {
        // Target velocity
        Vector3 targetVelocity = moveInput * moveSpeed;

        // Keep current vertical velocity (gravity/jumps)
        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);

        // Smooth acceleration (Stumble Guys feel)
        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);

        // Optional: rotate towards movement direction
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 10f));
        }
    }

    private void OnCollisionStay(Collision other)
    {
        // Simple grounded check
        foreach (var contact in other.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}
