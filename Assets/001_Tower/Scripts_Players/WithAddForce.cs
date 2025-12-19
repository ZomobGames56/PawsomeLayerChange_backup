using UnityEngine;

public class WithAddForce : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] Transform cam;

    [Header("Jump & Dash")]
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float dashForce = 15f;
    [SerializeField] float dashDuration = 0.3f;
    [SerializeField] float dashCooldown = 1f;

    Rigidbody rb;
    Animator animator;

    float horizontal, vertical;
    Vector3 desiredDir;
    bool isGrounded;

    float lastSpaceTime;
    bool isDashing;
    float dashTimer;
    float dashCDTimer;
    [SerializeField]
    float force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ|RigidbodyConstraints.FreezeRotationY;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDashing) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cam.forward; camForward.y = 0; camForward.Normalize();
        Vector3 camRight = cam.right; camRight.y = 0; camRight.Normalize();

        desiredDir = (camRight * horizontal + camForward * vertical).normalized;

        if (desiredDir != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(desiredDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 720f * Time.deltaTime);
        }

        HandleJump();
       // HandleDash();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = transform.forward * dashForce;
            return;
        }

        // 🚀 Your replacement code here:
        Vector3 desiredVelocity = desiredDir * moveSpeed;
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.y = 0;
        Vector3 velocityChange = desiredVelocity - currentVelocity;
        velocityChange = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);
      //  velocityChange.y = 0; // don’t affect vertical

        rb.AddForce(velocityChange * acceleration, ForceMode.VelocityChange);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSpaceTime <= 0.3f && dashCDTimer <= 0f)
            {
               // StartDash();
            }
            else if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            lastSpaceTime = Time.time;
        }
    }

    void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                dashCDTimer = dashCooldown;
                ResetTilt();
            }
        }
        else if (dashCDTimer > 0f)
        {
            dashCDTimer -= Time.deltaTime;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;

        // quick dive tilt
        transform.rotation *= Quaternion.Euler(70f, 0, 0);
    }

    void ResetTilt()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;

        if (collision.collider.CompareTag("Jumper"))
        {
            // Vector3 y = collision.collider.transform.up * force;
            rb.AddForce(Vector3.up*force, ForceMode.Impulse);
           
           // rb.AddForce(RBVelocity * force, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

}
