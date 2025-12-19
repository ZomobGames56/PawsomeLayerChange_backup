using UnityEngine;

public class PlayerMoveTEst_1 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Transform cam;

    [Header("Jump")]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float upForce = 15f;

    Rigidbody rb;
    Animator animator;

    Vector3 move;
    float horizontal, vertical;
    bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Important for character controllers
        rb.freezeRotation = true;
    }

    private void Update()
    {
        ReadInput();
        RotatePlayer();
        JumpInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // ---------------- INPUT ----------------
    void ReadInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        move = camRight * horizontal + camForward * vertical;
        move = Vector3.ClampMagnitude(move, 1f);
    }

    // ---------------- MOVEMENT ----------------
    void MovePlayer()
    {
        Vector3 velocity = rb.linearVelocity;

        if (move.sqrMagnitude > 0.01f)
        {
            velocity.x = move.x * moveSpeed;
            velocity.z = move.z * moveSpeed;
        }
        else
        {
            // Hard stop when input released
            velocity.x = 0f;
            velocity.z = 0f;
        }

        rb.linearVelocity = velocity;
    }

    // ---------------- ROTATION ----------------
    void RotatePlayer()
    {
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                720f * Time.deltaTime
            );
        }
    }

    // ---------------- JUMP ----------------
    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // ---------------- COLLISIONS ----------------
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.collider.CompareTag("Jumper"))
        {
            Vector3 launchDir =
                collision.transform.forward * upForce +
                collision.transform.up * upForce;

            rb.AddForce(launchDir, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
