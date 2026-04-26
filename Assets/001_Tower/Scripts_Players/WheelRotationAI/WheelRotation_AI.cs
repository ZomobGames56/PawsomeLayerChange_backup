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
    private Rigidbody[] childRbs;
    public bool isGrounded;
    public bool canMoveTowardTarget;

    [SerializeField] float stoppingDis = 1f;
    public bool once = true;

    Transform previousMoveTarget, startMoveTarget;

    [SerializeField] GameObject dummyPanel;
    public TextMeshProUGUI looseText;
    public int deadCout = 0;
    [SerializeField] float checkRadius = 0.5f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float maxAdjustDistance = 2f;

    public Transform hip;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        childRbs = GetComponentsInChildren<Rigidbody>();

        animator = GetComponent<Animator>();

        rb.freezeRotation = true;
        canMoveTowardTarget = true;
        deadCout = 0;
        // store initial target
        previousMoveTarget = moveTarget;
        startMoveTarget = moveTarget;
        moveForce = Random.Range(3, 6);
        EnableKinematic();
    }

    void Update()
    {
        if (!HY_StartPause.countOver) return;
        // 🔻 FALL CHECK
        if (transform.position.y < fallYLimit)
        {
            Respawn();
            return;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            EnemyRagdoll();
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
        Vector3 rot = (moveTarget.position - transform.position).normalized;
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
        rb.isKinematic = false;
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
        Vector3 spawnPos = GetSafeSpawnPosition();

        transform.position = spawnPos;
        transform.rotation = spawnPoint.rotation;

        moveTarget = startMoveTarget;
        canMoveTowardTarget = true;
        once = true;
        isGrounded = true;

        animator.SetBool("Jump", false);
        animator.SetBool("Hanging", false);
        animator.SetFloat("Run", 0f);
    }

    Vector3 GetSafeSpawnPosition()
    {
        Vector3 basePos = spawnPoint.position;

        // Step 1: Check if initial position is free
        if (!Physics.CheckSphere(basePos, checkRadius, obstacleLayer))
        {
            return SnapToGround(basePos);
        }

        // Step 2: Try nearby positions
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-maxAdjustDistance, maxAdjustDistance),
                0,
                Random.Range(-maxAdjustDistance, maxAdjustDistance)
            );

            Vector3 testPos = basePos + randomOffset;

            if (!Physics.CheckSphere(testPos, checkRadius, obstacleLayer))
            {
                return SnapToGround(testPos);
            }
        }

        // fallback (if everything fails)
        return SnapToGround(basePos);
    }
    // Collision with the obstacle---> don't allow another collsion--> Call Respawn-->
    // if more than 2 sec respawn else fall logic gonna call just have to reset the enemy by turning all the things back


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shovel") && once)
        {
            Debug.Log("Collide" + gameObject.name);
            once = false;
            //GetComponent<NavMeshAgent>().enabled = false;
            rb.isKinematic = true;
            transform.SetParent(null);
            EnemyRagdoll();


        }
    }
    #region
    public void EnemyRagdoll()
    {
        StopAllCoroutines();
        once = false;
        animator.enabled = false;

        DisableKinematic();

        StartCoroutine(ResetRagdoll());
    }

    private IEnumerator ResetRagdoll()
    {
        yield return new WaitForSeconds(3f);
        EnableKinematic();
        animator.enabled = true;
        if (!once)
            Respawn();
    }

    // ---------------- RIGIDBODY HELPERS ----------------

    private void EnableKinematic()
    {
        foreach (var rb in childRbs)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        this.rb.isKinematic = false;
        this.rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void DisableKinematic()
    {
        foreach (var rb in childRbs)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
        }
        this.rb.isKinematic = true;
    }
    #endregion

    Vector3 SnapToGround(Vector3 pos)
    {
        RaycastHit hit;

        if (Physics.Raycast(pos + Vector3.up * 2f, Vector3.down, out hit, 5f, groundLayer))
        {
            return hit.point;
        }

        return pos; // fallback
    }
}



