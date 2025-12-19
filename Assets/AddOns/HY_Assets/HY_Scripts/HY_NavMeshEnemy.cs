using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class HY_NavMeshEnemy : MonoBehaviour
{
    [SerializeField]
    Transform target, playerTrans;
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    public float rndSpeed = 7.0f, onLinkSpeed = 2;
    HY_Player_Control plc;
    [SerializeField]
    Animator enmyAnim;
    public bool canMove;
    public bool touchedFinishLine;
    //[SerializeField]
    //float time = 0;//, changeSpeedFloat = 10;
    public bool followPath;
    //public static bool goRagdoll = false;
    Rigidbody rb;
    bool callOnce;
    [SerializeField]
    float onSliderSpeed;
    [SerializeField]
    float heightIncrease = 2.0f;
    void Start()
    {
        //goRagdoll = false;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        followPath = true;
        plc = GetComponent<HY_Player_Control>();
        enmyAnim = GetComponentInChildren<Animator>();
        if (enmyAnim == null)
        {
            enmyAnim = GetComponent<Animator>();
        }
        canMove = true;
        callOnce = true;
    }

    void Update()
    {
        if (HY_StartPause.countOver)
        {
            if (canMove == true)
            {
                if (followPath == true && callOnce == true)
                {
                    agent.SetDestination(target.position);
                    callOnce = false;
                }

                enmyAnim.SetFloat("Run", agent.velocity.sqrMagnitude);

                if (agent.isOnOffMeshLink)
                {
                    Vector3 targetPosition = agent.transform.position;
                    targetPosition.y += heightIncrease;
                    agent.speed = onLinkSpeed;
                    enmyAnim.ResetTrigger("Dashing");
                    enmyAnim.SetBool("Dash", false);
                    enmyAnim.SetBool("Jump", true);
                    enmyAnim.SetBool("Hanging", true);
                }
                else
                {
                    agent.speed = rndSpeed;
                    enmyAnim.SetBool("Jump", false);
                    enmyAnim.SetBool("Hanging", false);
                }
                //if (Vector3.Distance(transform.position, target.position) <= agent.radius)
                //{
                //    agent.ResetPath();
                //}
            }
            else
            {
                agent.speed = 0;
               // agent.isStopped = true;
            }


        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Slider")
        {
            enmyAnim.SetTrigger("Dashing");
            enmyAnim.SetBool("Dash", true);
            agent.speed = onSliderSpeed;
        }
        //if (collision.transform.tag == "Jumper")
        //{
        //    rb.AddForce(Vector3.up * 23f, ForceMode.Impulse);


        //}


    }
    void AISpeedChange()
    {
        // rndSpeed = Random.Range(3, 9);

    }
    public void SetDestinationTarget()
    {
        agent.SetDestination(target.position);
    }

   
    public Color pathColor = Color.green;
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
