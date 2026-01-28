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
    public int deadCout =0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;
        canMoveTowardTarget = true;
        deadCout= 0;
        // store initial target
        previousMoveTarget = moveTarget;
        startMoveTarget = moveTarget;
        moveForce = Random.Range(3, 6);
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

        if (deadCout >= 5)
        {
            forwardForce = 10.5f;
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
        Vector3 rot = (moveTarget.position-transform.position).normalized;
        transform.rotation = Quaternion.Euler(rot.normalized);

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
            looseText.text = "Lose";
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
        deadCout++;
        moveForce = Random.Range(3, 6);

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



