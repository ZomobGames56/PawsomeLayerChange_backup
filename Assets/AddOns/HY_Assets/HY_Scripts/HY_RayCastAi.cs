using UnityEngine;

public class HY_RayCastAi : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform m_transform;
    [SerializeField] private LayerMask layer;
    Rigidbody rb;
    [SerializeField]
    float moveSpeed = 5f, force = 7f;
    bool rotateOnce = false;
    RaycastHit hit;
    [SerializeField] private float maxdis = 100f;
    float time;
    Animator animator;
    Vector3 randomDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    //void movement()
    //{
    //    float x = Input.GetAxis("Horizontal");
    //    float z = Input.GetAxis("Vertical");
    //    Vector3 dir = (transform.right * x + transform.forward * z).normalized;
    //    transform.position += dir * moveSpeed * Time.deltaTime;
    //}
    private void Update()
    {
        if (HY_StartPause.countOver)
        {
            if (Physics.Raycast(m_transform.position, m_transform.forward, out hit, maxdis))
            {
                // if (!rotateOnce)

                if (hit.collider.tag == "Wall")
                {
                    Quaternion randomDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0);// * transform.forward;
                    transform.rotation = Quaternion.Slerp(transform.rotation, randomDirection, 100f);
                    // rb.MoveRotation(Quaternion.LookRotation(randomDirection));
                }

                Debug.DrawLine(m_transform.position, hit.point, Color.red);
            }
            else
            {
                Vector3 randomDirection = Quaternion.Euler(Random.Range(-15f, 15f), Random.Range(-15f, 15f), 0) * transform.forward;
                animator.SetFloat("Run", randomDirection.magnitude);
                rb.MovePosition(transform.position + randomDirection * moveSpeed * Time.deltaTime);
                animator.SetBool("Jump", false);
                animator.SetBool("Hanging", false);
                time += Time.deltaTime;
                if (time > 1f)
                {
                    if (!rotateOnce)
                    {
                        rotateOnce = true;
                        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
                        animator.SetBool("Jump", true);
                        animator.SetBool("Hanging", true);
                        //rb.MoveRotation(Quaternion.LookRotation(RandoRoatation()));
                    }
                    rotateOnce = false;
                    time = 0;
                }

                Debug.DrawLine(m_transform.position, m_transform.forward * maxdis, Color.green);
            }
        }

    }
    Vector3 RandoRoatation()

    {
        return randomDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;
    }

}
