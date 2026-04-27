using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    [Header("Mobile Optimization")]
    public Transform player;
    public float disableDistance = 30f;

    private NavMeshAgent agent;
    private int currentWaypointIndex;
    private float updateTimer;

    private const int MAX_SAMPLE_ATTEMPTS = 2;
    private int sampleAttempts;

    private enum AIState { Moving, GoalReached, Ragdoll }
    private AIState currentState = AIState.Moving;

    private float arrivalSqr;
    private float disableSqr;
    [SerializeField]
    GameObject dummyScreen;
    //private void OnEnable()
    //{
    //    mn_GameManager.OnWinEvent += StopMovement;
    //}
    //private void OnDisable()
    //{
    //    mn_GameManager.OnWinEvent -= StopMovement;
    //}
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.autoBraking = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        agent.updateRotation = false;

        arrivalSqr = waypointArrivalDistance * waypointArrivalDistance;
        disableSqr = disableDistance * disableDistance;

        if (pathWaypoints == null || pathWaypoints.Length == 0)
        {
            enabled = false;
            return;
        }

        //UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
    }

    private void Update()
    {
        if (!HY_StartPause.countOver) return;
        // 🔴 Mobile AI sleep when far
        if (player &&
            (transform.position - player.position).sqrMagnitude > disableSqr)
            return;

        switch (currentState)
        {
            case AIState.Moving:
                HandleMoving();
                break;
        }
    }

    // ---------------- MOVEMENT ----------------

    private void HandleMoving()
    {
        Vector3 wpPos = pathWaypoints[currentWaypointIndex].position;

        if ((transform.position - wpPos).sqrMagnitude <= arrivalSqr)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= pathWaypoints.Length)
            {
                agent.isStopped = true;
                currentState = AIState.GoalReached;
                //call the event.
                //mn_GameManager.Won();
                return;
            }

            UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
            updateTimer = 0f;
        }

        updateTimer -= Time.deltaTime;
        if (!agent.isOnNavMesh) return;
        if (updateTimer <= 0f || (!agent.pathPending && agent.remainingDistance < 1f))//here error
        {
            UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);// here error
            updateTimer = targetUpdateInterval * Random.Range(0.8f, 1.2f);
        }

        UpdateAnimation();
        RotateToVelocity();
    }

    // ---------------- RANDOM TARGET ----------------

    private void UpdateRandomTarget(Vector3 goal)
    {
        if (sampleAttempts++ > MAX_SAMPLE_ATTEMPTS)
        {
            agent.SetDestination(goal);
            sampleAttempts = 0;
            return;
        }

        Vector3 direction = (goal - transform.position).normalized;
        Vector3 baseTarget = transform.position + direction * randomStepDistance;

        if (!NavMesh.SamplePosition(baseTarget, out NavMeshHit baseHit,
            randomStepDistance * 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(goal);
            return;
        }

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up);
        Vector3 randomTarget = baseHit.position +
                               perpendicular * Random.Range(-maxLateralWiggle, maxLateralWiggle);

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
        if (!animator) return;

        float speed01 = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Run", speed01, 0.1f, Time.deltaTime);
    }

    private void RotateToVelocity()
    {
        Vector3 vel = agent.velocity;
        vel.y = 0;

        if (vel.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(vel),
                Time.deltaTime * 8f
            );
    }

    // ---------------- RAGDOLL ----------------

    public void EnterRagdollState()
    {
        currentState = AIState.Ragdoll;
        //agent.isStopped = true;
        //agent.ResetPath();

        if (animator) animator.enabled = false;
    }
   
    
    public void ExitRagdollState()
    {
        if (currentState == AIState.GoalReached) return;

        currentState = AIState.Moving;

        if (animator) animator.enabled = true;

        UpdateRandomTarget(pathWaypoints[currentWaypointIndex].position);
        agent.isStopped = false;
    }
}