using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnpredictableClimber : MonoBehaviour
{
    // --- Public Variables to Set in Inspector ---

    [Header("Path Progression")]
    public Transform[] pathWaypoints;       // Waypoints defining the climb order (Bottom to Top)
    public float waypointArrivalDistance = 3.0f; // How close AI must get to a waypoint to proceed to the next

    [Header("Random Walk Settings")]
    public float targetUpdateInterval = 1.0f;   // How often (in seconds) to pick a new random sub-goal
    public float randomStepDistance = 5.0f;     // How far the random sub-goal can be generated
    public float maxLateralWiggle = 3.0f;       // Max distance the random point can be offset sideways

    // --- Private Components & State ---
    private NavMeshAgent agent;

    private int currentWaypointIndex = 0;
    private float updateTimer = 0f;

    private enum AIState { Moving, GoalReached }
    private AIState currentState = AIState.Moving;
    // -- Animator Compoenet --
    [Header("Animator")]
    [SerializeField]
    Animator animator;
    float animationDamp = 0.1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.autoBraking = false;

        if (pathWaypoints.Length == 0)
        {
            Debug.LogError("Path Waypoints not assigned! Please define the path in the Inspector.");
            enabled = false;
        }
        else
        {
            UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case AIState.Moving:
                HandleMoving();
                break;
            case AIState.GoalReached:
                break;
        }
    }

    // --- State Handler ---

    private void HandleMoving()
    {
        // 1. Check for Waypoint Arrival
        if (Vector3.Distance(transform.position, pathWaypoints[currentWaypointIndex].position) <= waypointArrivalDistance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex < pathWaypoints.Length)
            {
                Debug.Log($"Waypoint {currentWaypointIndex - 1} reached. Moving to Waypoint {currentWaypointIndex}.");
                UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
                updateTimer = 0f; // Reset timer for immediate update
                return;
            }
            else
            {
                // Goal Reached
                Debug.Log("**Final Waypoint Reached! Climb Complete.**");
                agent.isStopped = true;
                currentState = AIState.GoalReached;
                return;
            }
        }


        // 2. Random Walk Logic
        updateTimer -= Time.deltaTime;

        // If timer is up OR we reached the current random sub-goal
        if (updateTimer <= 0f || !agent.pathPending && agent.remainingDistance < 1.0f)
        {
            UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
            updateTimer = targetUpdateInterval;
        }
        if (animator == null) return;

        UpdateAnimation();
    }

    // --- Core Randomization Logic (NavMesh Safe and Forward-Only) ---

    /// <summary>
    /// Generates a safe, random sub-goal that enforces forward progression toward the high-level waypoint.
    /// </summary>
    private void UpdateRandomTarget(Vector3 highLevelGoal)
    {
        // 1. Find the general direction towards the high-level goal
        // This vector *must* be calculated from the current position to the goal to enforce forward movement.
        Vector3 directionToGoal = (highLevelGoal - transform.position).normalized;

        // 2. Create a base point *ahead* of the agent
        Vector3 baseTarget = transform.position + directionToGoal * randomStepDistance;

        NavMeshHit baseHit;
        // Safety Check 1: Find a valid point near the calculated forward position
        if (NavMesh.SamplePosition(baseTarget, out baseHit, randomStepDistance * 2f, NavMesh.AllAreas))
        {
            baseTarget = baseHit.position;
        }
        else
        {
            // Fallback: If a forward step is unsafe, aim directly for the final waypoint
            agent.SetDestination(highLevelGoal);
            Debug.LogWarning("Forward step failed. Aiming directly for waypoint.");
            return;
        }

        // --- Apply Lateral Wiggle to the Valid Base Point ---
        Vector3 finalDirection = (baseTarget - transform.position).normalized;

        // 3. Calculate a sideways vector (perpendicular to the path on the XZ plane)
        // This ensures the wiggle is perpendicular to the direction of travel.
        Vector3 perpendicular = Vector3.Cross(finalDirection, Vector3.up).normalized;

        // 4. Apply a random lateral offset (The Wiggle)
        float randomOffset = Random.Range(-maxLateralWiggle, maxLateralWiggle);
        Vector3 randomTarget = baseTarget + (perpendicular * randomOffset);


        // 5. Safety Check 2: Ensure the Wiggle point is still on the NavMesh
        NavMeshHit finalHit;
        if (NavMesh.SamplePosition(randomTarget, out finalHit, maxLateralWiggle * 2f, NavMesh.AllAreas))
        {
            // Final check: Ensure the new target isn't behind the AI's current forward vector
            // This is implicitly handled by starting the calculation with directionToGoal, but this dot product provides a clear check.
            if (Vector3.Dot(finalHit.position - transform.position, directionToGoal) < 0)
            {
                // The calculated point is behind us relative to the goal direction, use the safer base target.
                agent.SetDestination(baseTarget);
            }
            else
            {
                // The random point is valid and forward. Set it as the destination.
                agent.SetDestination(finalHit.position);
            }
        }
        else
        {
            // Fallback: If the wiggle is unsafe, aim for the safe, center base point
            agent.SetDestination(baseTarget);
        }
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Run", speed, animationDamp, Time.deltaTime);
    }
}