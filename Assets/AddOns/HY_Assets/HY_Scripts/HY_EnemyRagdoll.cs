using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HY_EnemyRagdoll : MonoBehaviour, IHitAble
{
    [Header("References")]
    public GameObject Parent;                 // Main enemy root
    public Transform hip;                     // Hip bone
    public UnpredictableClimber movementAI;   // Movement script reference

    private Rigidbody[] childRbs;
    private Animator animator;
    private NavMeshAgent agent;

    Rigidbody _Hip;
    private void Awake()
    {
        childRbs = GetComponentsInChildren<Rigidbody>();
        animator = Parent.GetComponent<Animator>();
        agent = Parent.GetComponent<NavMeshAgent>();
        _Hip = GetComponent<Rigidbody>();
        EnableKinematic();
    }

    // ---------------- RAGDOLL CONTROL ----------------
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            EnemyRagdoll();
        }
    }
    public void EnemyRagdoll()
    {
        StopAllCoroutines();

        // Disable AI + animation
        if (movementAI != null)
            movementAI.EnterRagdollState();

        animator.enabled = false;
        agent.enabled = false;

        DisableKinematic();

        StartCoroutine(ResetRagdoll());
    }

    private IEnumerator ResetRagdoll()
    {
        yield return new WaitForSeconds(3f);

        // Snap character back to hips
        Parent.transform.position = hip.position;

        EnableKinematic();

        animator.enabled = true;
        agent.enabled = true;
        agent.velocity = Vector3.zero;

        if (movementAI != null)
            movementAI.ExitRagdollState();
    }

    // ---------------- RIGIDBODY HELPERS ----------------

    private void EnableKinematic()
    {
        foreach (var rb in childRbs)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void DisableKinematic()
    {
        foreach (var rb in childRbs)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void ApplyKnoackBackForce(Vector3 knockBackDirection, float impactForce, float ImpactMultiplier)
    {
        EnemyRagdoll();
        _Hip.AddForce(knockBackDirection * impactForce * ImpactMultiplier, ForceMode.Impulse);
    }
}
