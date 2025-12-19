using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WaypointMover : MonoBehaviour
{
    public Transform[] waypoints;  // List of waypoints
    NavMeshAgent agent;
    int currentIndex;
    [SerializeField]
    float rotSpeed=2.5f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentIndex = 0;
        agent.SetDestination(waypoints[currentIndex].position);

    }
    private void Update()
    {
        transform.Rotate(-rotSpeed * Time.deltaTime,0,0);   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "BallWayPoints")
        {
            if (currentIndex < waypoints.Length - 1)
            {
                currentIndex++;
            }
            agent.SetDestination(waypoints[currentIndex].position);
            if (currentIndex >= waypoints.Length - 1)
            {
                currentIndex = waypoints.Length - 1;
                print(currentIndex);
            }
        }
    }
}
