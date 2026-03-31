using UnityEngine;
using UnityEngine.AI;

public class TowerLevelAI : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Transform[] wayPoints;
    int currentIndex = 0;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(wayPoints[currentIndex].position); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "WayPoints")
        {
            if (currentIndex < wayPoints.Length - 1)
            {
                currentIndex++;
                print(currentIndex);
            }
            agent.SetDestination(wayPoints[currentIndex].position);
            if (currentIndex >= wayPoints.Length - 1)
            {
                currentIndex = wayPoints.Length - 1;
            }
        }
    }
}
