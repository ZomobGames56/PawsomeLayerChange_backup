using UnityEngine;
using UnityEngine.AI;

public class NavMeshM0vement : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        // Start the first random movement
        SetRandomDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Check if the agent has reached its destination
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (timer >= wanderTimer)
            {
                SetRandomDestination();
                timer = 0;
            }
        }
    }

    void SetRandomDestination()
    {
        // Pick a random position within the specified radius
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
