using System.IO;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class TestNAvMeshAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform destinationtarget;
    NavMeshAgent agent;
    private NavMeshPath path;
    public Color pathColor = Color.green;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinationtarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            UpdateTarget();
        }
    }
    void UpdateTarget()
    {
        agent.SetDestination(destinationtarget.position);
    }
    void OnDrawGizmos()
    {
        // Make sure we have a NavMeshAgent assigned
        if (agent == null)
            return;

        // Get the path of the agent
        NavMeshPath path = agent.path;

        // Set the color of the gizmo line
        Gizmos.color = pathColor;

        // Loop through the corners (waypoints) of the path
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            // Draw a line between each corner
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        }
    }
}

