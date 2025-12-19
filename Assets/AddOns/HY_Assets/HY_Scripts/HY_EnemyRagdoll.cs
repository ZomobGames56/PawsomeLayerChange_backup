using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HY_EnemyRagdoll : MonoBehaviour
{
    public static HY_EnemyRagdoll instance;
    Rigidbody[] childRbs;
    Animator animator;
    private NavMeshAgent agent_Ref;
    [SerializeField]
    Transform hip;
    
    public GameObject Parent;
    //public HY_NavMeshEnemy _refNavMesh;
    public NavMeshWithWayPointsAI _refEnemy;
    void Awake()
    {
       // _refNavMesh = GetComponentInParent<HY_NavMeshEnemy>();
       _refEnemy= GetComponentInParent<NavMeshWithWayPointsAI>();
        childRbs = GetComponentsInChildren<Rigidbody>();
        EnableKinamatic();
        animator = GetComponentInParent<Animator>();
        agent_Ref = GetComponentInParent<NavMeshAgent>();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            agent_Ref.enabled = false;
            EnemyRagdoll();
        }
    }
    void EnableKinamatic()
    {
        foreach (var child in childRbs)
        {
            child.isKinematic = true;
            child.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    public void DisableKinamatic()
    {
        foreach (var child in childRbs)
        {
            child.isKinematic = false;
            child.constraints = RigidbodyConstraints.None;
        }
    }
    public IEnumerator ResetRagoll()
    {
        yield return new WaitForSeconds(3f);
       // HY_NavMeshEnemy.goRagdoll = false;
        Parent.transform.position = transform.position;
        animator.enabled = true;
       // _refEnemy.SetDestination();
        //_refEnemy.followPath = true;
        foreach (var child in childRbs)
        {
            child.isKinematic = true;
            child.constraints = RigidbodyConstraints.FreezeAll;
        }


    }
    //  Must Use Event here........
    public void EnemyRagdoll()
    {
        // Debug.Log("Collided to the obstacle");
        animator.enabled = false;
        agent_Ref.speed = 0;
        
        // followPath = false;
       // _refEnemy.followPath = false;
        DisableKinamatic();
        StartCoroutine(ResetRagoll());
    }
    //[System.Obsolete]
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.tag == "Obstacle" && _refEnemy != null)
    //    {
          
    //        animator.enabled = false;
    //        agent_Ref.speed = 0;
          
    //        DisableKinamatic();
    //        StartCoroutine(ResetRagoll());
          
    //    }
    //}
}
