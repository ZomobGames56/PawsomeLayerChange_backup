using UnityEngine;
using UnityEngine.XR;

public class MyPlayerTestDash : MonoBehaviour
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

    Vector3 moveInput;
    bool isGrounded;

    float lastSpaceTime;
    bool isDashing;
    float dashTimer;
    float dashCDTimer;

    Quaternion targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // don�t tip over
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDashing) return; // block input while dashing

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cam.forward; camForward.y = 0; camForward.Normalize();
        Vector3 camRight = cam.right; camRight.y = 0; camRight.Normalize();

        moveInput = (camForward * v + camRight * h).normalized;

        if (moveInput != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.deltaTime);
        }

        HandleJump();
        //  HandleDash();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = transform.forward * dashForce;
            return;
        }

        // Smooth acceleration toward moveInput
        Vector3 desiredVelocity = moveInput * moveSpeed;
        Vector3 velocityChange = desiredVelocity - rb.linearVelocity;
        velocityChange.y = rb.linearVelocity.y; // don�t mess with gravity

        //rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
        // rb.AddForce(new Vector3(velocityChange.x, 0, velocityChange.z), ForceMode.VelocityChange);
        rb.linearVelocity = new Vector3(velocityChange.x, velocityChange.y, velocityChange.z);
    }
    private void LateUpdate()
    {
       
    }
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // check double tap
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

        // tilt character on X axis (like stumble dive)
        transform.rotation *= Quaternion.Euler(70f, 0, 0);

        // optional: reset tilt after dash
        //  Invoke(nameof(ResetTilt), dashDuration);
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

            Vector3 dir = collision.collider.transform.up;
            print(dir);
            rb.AddForce(dir*50f, ForceMode.Impulse);
               
        }
          
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

}
