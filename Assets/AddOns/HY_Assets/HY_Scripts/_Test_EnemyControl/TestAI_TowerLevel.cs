using UnityEngine;
using UnityEngine.AI;

public class TestAI_TowerLevel : MonoBehaviour
{
  

    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform target;
   

    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        agent.SetDestination(target.position);
    }
   
}
