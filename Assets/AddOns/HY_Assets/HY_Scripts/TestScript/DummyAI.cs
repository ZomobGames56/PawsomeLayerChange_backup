using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DummyAI : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]
    Transform target;
    [SerializeField]
    float maxDistance;
    RaycastHit hit;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
        
    }
    private void Update()
    {

        if (Physics.Raycast(transform.position, Vector3.forward, out hit,maxDistance))
        {
            if (hit.collider != null)
            {
                agent.SetDestination(target.position);
                agent.autoRepath = true;
                Debug.Log("Hit Target",gameObject);
            }
                Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }

    
}
