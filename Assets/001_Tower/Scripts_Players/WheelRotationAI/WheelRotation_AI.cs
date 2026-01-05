//using System.Collections;
//using TMPro;
//using UnityEngine;

//public class WheelRotation_AI : MonoBehaviour
//{
//    public Transform jumpTarget, jumpInfo;
//    public float upForce = 6f;
//    public float forwardForce = 4f, moveForce = 5f;
//    public Transform moveTarget;
//    Rigidbody rb;
//   public bool isGrounded;
//   public bool canMoveTowardTarget;
//    [SerializeField]
//    float stoppingDis = 1f;
//   public bool once = true;
//    int currentInx = 0;
//    [SerializeField]
//    GameObject dummyPanel;
//    public TextMeshProUGUI looseText;
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true;
//        canMoveTowardTarget = true;
//        currentInx = 0;

//    }

//    void Update()
//    {

//        if (moveTarget != null && Vector3.Distance(transform.position, moveTarget.position) < stoppingDis)
//        {
//            canMoveTowardTarget = false;
//        }
//        if (moveTarget != null && canMoveTowardTarget == true && isGrounded)
//            MoveTowards();
//    }

//    void Jump()
//    {
//        if (!isGrounded) return;

//        Vector3 toward = (jumpTarget.position - transform.position).normalized;
//        toward.y = 0;

//        Vector3 jumpVector = toward * forwardForce + Vector3.up * upForce;

//        rb.linearVelocity = Vector3.zero;
//        rb.AddForce(jumpVector, ForceMode.Impulse);

//    }
//    void MoveTowards()
//    {
//        if (!isGrounded) return;

//        Vector3 dir = (moveTarget.position - transform.position).normalized;
//        dir.y = 0;
//        transform.LookAt(dir);
//        rb.linearVelocity = new Vector3(dir.x * moveForce, rb.linearVelocity.y, dir.z * moveForce);

//    }
//    void FixedUpdate()
//    {
//        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "JumpDetect" && once)
//        {
//            Jump();
//            once = false;
//            canMoveTowardTarget = false;
//            if (other.transform.childCount > 0)
//            {
//                moveTarget = other.transform.GetChild(0);
//            }
//            //else
//            //{
//            //    moveTarget = null;
//            //}
//            //other.gameObject.SetActive(false);
//            StartCoroutine(ChangeMoveTarget());
//        }
//        if (other.tag == "Goal")
//        {
//            looseText.text = "Loose";
//            dummyPanel.SetActive(true);
//            Time.timeScale = 0;
//        }
//    }
//    //bool JumpDetect()
//    //{
//    //    return Vector3.Distance(transform.position, jumpInfo.position) < 10f;
//    //}

//    IEnumerator ChangeMoveTarget()
//    {
//        yield return new WaitForSeconds(1f);
//        // currentInx++;
//        canMoveTowardTarget = true;
//        once = true;

//    }
//}

using System.Collections;
using TMPro;
using UnityEngine;

public class WheelRotation_AI : MonoBehaviour
{
    public Transform jumpTarget;
    public float upForce = 6f;
    public float forwardForce = 4f, moveForce = 5f;
    public Transform moveTarget;

    [Header("Respawn")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] float fallYLimit = -10f;

    Rigidbody rb;
    Animator animator;

    public bool isGrounded;
    public bool canMoveTowardTarget;

    [SerializeField] float stoppingDis = 1f;
    public bool once = true;

    Transform previousMoveTarget,startMoveTarget;

    [SerializeField] GameObject dummyPanel;
    public TextMeshProUGUI looseText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;
        canMoveTowardTarget = true;

        // store initial target
        previousMoveTarget = moveTarget;
        startMoveTarget = moveTarget;
    }

    void Update()
    {
        // 🔻 FALL CHECK
        if (transform.position.y < fallYLimit)
        {
            Respawn();
            return;
        }

        UpdateAnimations();

        if (moveTarget != null && Vector3.Distance(transform.position, moveTarget.position) < stoppingDis)
        {
            canMoveTowardTarget = false;
        }

        if (moveTarget != null && canMoveTowardTarget && isGrounded)
        {
            MoveTowards();
        }
    }

    // ===================== ANIMATION =====================
    void UpdateAnimations()
    {
        animator.SetBool("Jump", !isGrounded);
        animator.SetBool("Hanging", !isGrounded);

        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        animator.SetFloat("Run",
            (isGrounded && canMoveTowardTarget && speed > 0.1f) ? 1f : 0f);
    }

    // ===================== MOVEMENT =====================
    void MoveTowards()
    {
        if (!isGrounded) return;

        Vector3 dir = moveTarget.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.01f) return;

        transform.rotation = Quaternion.LookRotation(dir);

        rb.linearVelocity = new Vector3(
            dir.normalized.x * moveForce,
            rb.linearVelocity.y,
            dir.normalized.z * moveForce
        );
    }

    void Jump()
    {
        if (!isGrounded) return;

        Vector3 toward = (jumpTarget.position - transform.position).normalized;
        toward.y = 0;

        transform.rotation = Quaternion.LookRotation(toward);

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(toward * forwardForce + Vector3.up * upForce, ForceMode.Impulse);
    }

    // ===================== GROUND CHECK =====================
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
            isGrounded = false;
    }

    // ===================== TRIGGERS =====================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpDetect") && once)
        {
            once = false;
            canMoveTowardTarget = false;

            previousMoveTarget = moveTarget; // 🔥 SAVE OLD TARGET

            if (other.transform.childCount > 0)
            {
                jumpTarget = other.transform.GetChild(0);
                moveTarget = jumpTarget;
            }

            Jump();
            StartCoroutine(ChangeMoveTarget());
        }

        if (other.CompareTag("Goal"))
        {
            looseText.text = "Loose";
            dummyPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator ChangeMoveTarget()
    {
        yield return new WaitForSeconds(1f);
        canMoveTowardTarget = true;
        once = true;
    }

    // ===================== RESPAWN =====================
    void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        moveTarget = startMoveTarget; // 🔥 RESTORE OLD TARGET
        canMoveTowardTarget = true;
        once = true;
        isGrounded = true;

        animator.SetBool("Jump", false);
        animator.SetBool("Hanging", false);
        animator.SetFloat("Run", 0f);
    }
}



