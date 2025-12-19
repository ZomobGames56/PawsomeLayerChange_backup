using System.Collections;
using TMPro;
using UnityEngine;

public class WoodenLV_AI_WithAnimation : MonoBehaviour
{
    [SerializeField] Transform jumpTarget, preDefinedJMT, testTarget;
    [SerializeField] float upForce = 6f;
    [SerializeField] float forwardForce = 4f, moveForce = 5f;
    [SerializeField] Transform moveTarget;

    Rigidbody rb;
    Animator animator;

    bool isGrounded;
    bool canMoveTowardTarget;
    bool once = true;

    [SerializeField] float stoppingDis = 1f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float runDampTime = 0.1f;

    [SerializeField] GameObject dummyPanel;
    [SerializeField] TextMeshProUGUI looseText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;
        canMoveTowardTarget = true;
    }

    void Update()
    {
        UpdateAnimations();

        if (moveTarget != null && canMoveTowardTarget && isGrounded)
        {
            MoveTowards();
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 3f);
    }

    // ===================== ANIMATION =====================
    void UpdateAnimations()
    {
        // Jump animation
        animator.SetBool("Jump", !isGrounded);
        animator.SetBool("Hanging", !isGrounded);

        // Run animation
        if (isGrounded && canMoveTowardTarget && rb.linearVelocity.magnitude > 0.1f)
        {
            animator.SetFloat("Run", 1f, runDampTime, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Run", 0f, runDampTime, Time.deltaTime);
        }
    }

    // ===================== MOVEMENT =====================
    void MoveTowards()
    {
        Vector3 dir = moveTarget.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );

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

        Quaternion targetRot = Quaternion.LookRotation(toward);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(toward * forwardForce + Vector3.up * upForce, ForceMode.Impulse);
    }

    // ===================== TRIGGERS =====================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpDetect") && once)
        {
            once = false;
            canMoveTowardTarget = false;

            if (other.transform.childCount > 0)
                jumpTarget = other.transform.GetChild(0);

            Jump();
            StartCoroutine(ChangeMoveTarget());
        }

        if (other.CompareTag("Goal"))
        {
            looseText.text = "Loose";
            dummyPanel.SetActive(true);
            Time.timeScale = 0;
        }

        if (other.CompareTag("PointIdentifier"))
        {
            canMoveTowardTarget = true;
            moveTarget = preDefinedJMT;
        }
    }

    IEnumerator ChangeMoveTarget()
    {
        yield return new WaitForSeconds(1f);
        moveTarget = testTarget;
        canMoveTowardTarget = true;
        once = true;
    }
}
