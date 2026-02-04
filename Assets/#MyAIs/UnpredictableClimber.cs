using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnpredictableClimber : MonoBehaviour
{
    [Header("Path")]
    public Transform[] pathWaypoints;
    public float waypointArrivalDistance = 3f;

    [Header("Random Walk")]
    public float targetUpdateInterval = 1f;
    public float randomStepDistance = 5f;
    public float maxLateralWiggle = 3f;

    [Header("Animator")]
    public Animator animator;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float updateTimer;

    private enum AIState { Moving, GoalReached, Ragdoll }
    private AIState currentState = AIState.Moving;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.autoBraking = false;

        if (pathWaypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned!");
            enabled = false;
            return;
        }

        UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
    }

    private void Update()
    {
        switch (currentState)
        {
            case AIState.Moving:
                HandleMoving();
                break;

            case AIState.Ragdoll:
                break;

            case AIState.GoalReached:
                break;
        }
    }

    // ---------------- MOVEMENT ----------------

    private void HandleMoving()
    {
        if (Vector3.Distance(transform.position,
            pathWaypoints[currentWaypointIndex].position) <= waypointArrivalDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= pathWaypoints.Length)
            {
                agent.isStopped = true;
                currentState = AIState.GoalReached;
                return;
            }

            UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
            updateTimer = 0f;
        }

        updateTimer -= Time.deltaTime;

        if (updateTimer <= 0f || (!agent.pathPending && agent.remainingDistance < 1f))
        {
            UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
            float jitter = Random.Range(0.8f, 1.2f);
            updateTimer = targetUpdateInterval * jitter;
           // updateTimer = targetUpdateInterval;
        }

        UpdateAnimation();
    }

    // ---------------- RANDOM TARGET ----------------

    private void UpdateRandomTarget(Vector3 goal)
    {
        Vector3 direction = (goal - transform.position).normalized;
        Vector3 baseTarget = transform.position + direction * randomStepDistance;

        if (!NavMesh.SamplePosition(baseTarget, out NavMeshHit baseHit,
            randomStepDistance * 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(goal);
            return;
        }

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;
        float offset = Random.Range(-maxLateralWiggle, maxLateralWiggle);
        Vector3 randomTarget = baseHit.position + perpendicular * offset;

        if (NavMesh.SamplePosition(randomTarget, out NavMeshHit finalHit,
            maxLateralWiggle * 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(finalHit.position);
        }
        else
        {
            agent.SetDestination(baseHit.position);
        }
    }

    // ---------------- ANIMATION ----------------

    private void UpdateAnimation()
    {
        if (animator == null) return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Run", speed, 0.1f, Time.deltaTime);
    }

    // ---------------- RAGDOLL API ----------------

    public void EnterRagdollState()
    {
        currentState = AIState.Ragdoll;
        agent.isStopped = true;
        agent.ResetPath();

        if (animator != null)
            animator.enabled = false;
    }

    public void ExitRagdollState()
    {
        if (currentState == AIState.GoalReached)
            return;

        currentState = AIState.Moving;

        if (animator != null)
            animator.enabled = true;

        agent.isStopped = false;
        UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
    }
}
