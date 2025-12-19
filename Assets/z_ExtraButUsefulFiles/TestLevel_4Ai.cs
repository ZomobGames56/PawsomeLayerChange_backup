using UnityEngine;

public class TestLevel_4Ai : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform faceTrans;
    Rigidbody rb;
    [SerializeField]
    float maxDistace, force = 10f;
    [SerializeField] private bool isGrounded;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ForwardRay();
        BackwardRay();
        DownRay();
    }
    void ForwardRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(faceTrans.position, faceTrans.forward, out hit, maxDistace))
        {
            if (isGrounded)
            {
                DoJump();
            }
            Debug.DrawLine(faceTrans.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(faceTrans.position, faceTrans.forward * maxDistace, Color.green);
        }
    }
    private void BackwardRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(faceTrans.position, -faceTrans.forward, out hit, maxDistace) && isGrounded)
        {
            //if (hit.collider.tag == "Wall")
            //{
            DoJump();
            // }
            Debug.DrawLine(faceTrans.position, hit.point, Color.red);
        }

    }
    void DownRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(faceTrans.position, Vector3.down, out hit, 2f))
        {
            if (hit.collider.tag == "Ground")
            {
                isGrounded = true;
                Debug.DrawLine(faceTrans.position, hit.point, Color.cyan);
            }
            else
            {
                isGrounded = false;
            }
        }
    }
    void DoJump()
    {
        isGrounded = false;
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        animator.SetBool("Jump", true);
        animator.SetBool("Hanging", true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
            animator.SetBool("Hanging", false);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("Jump", true);
            animator.SetBool("Jump", false);
            animator.SetBool("Hanging", true);
        }
    }
}
