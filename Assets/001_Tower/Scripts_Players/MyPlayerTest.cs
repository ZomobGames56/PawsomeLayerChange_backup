using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MyPlayerTest : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10.0f;
    Vector3 move;
    Rigidbody rb;
    float horizontal, vertical;
    Vector3 dir;
    [SerializeField]
    Transform cam;
    Quaternion rotation;
    Animator animator;
    [SerializeField] bool isGrounded;
    [SerializeField]
    float upForce = 15,jumpForce= 15;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 camRight = cam.right;
        Vector3 camForward = cam.forward;
        camForward.Normalize();
        camRight.Normalize();
        camForward.y = 0;
        camRight.y = 0;

        //move =  new Vector3(horizontal, 0, vertical).normalized;
        move = camRight * horizontal + camForward * vertical;
        // if move x change camera rotate slightly other side not too much 
        //move.y = 0;
        if (move.magnitude != 0)
        {
            Rotate();
            move = move.normalized;
        }


        JumpInput();
    }
    private void FixedUpdate()
    {
            Vector3 m_velocity = move * moveSpeed;
            //rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
            //m_velocity.y = rb.velocity.y;
            Debug.Log($"m_Velocity.Y: {rb.linearVelocity.y}");
            rb.linearVelocity =new Vector3(m_velocity.x,rb.linearVelocity.y,m_velocity.z);
         //print($"rb.velocity:{rb.velocity}");
    }

    void Rotate()
    {
        rotation = Quaternion.LookRotation(move, Vector3.up);
        //  transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100f * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 720f * Time.deltaTime);
    }
    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            //  this.enabled = false;
        }
    }
    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump Pressed");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
        }
        if (collision.collider.CompareTag("Jumper"))
        {
            Vector3 launchDir = collision.transform.forward * upForce + collision.transform.up* upForce;
            rb.AddForce(launchDir,ForceMode.VelocityChange);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
