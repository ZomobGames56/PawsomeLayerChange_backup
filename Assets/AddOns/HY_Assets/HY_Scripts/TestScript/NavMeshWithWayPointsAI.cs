using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshWithWayPointsAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform[] wayPonints;
    public NavMeshAgent agent;
    int currentIndex = 0;
    Animator enmyAnim;
    [SerializeField]
    public float onLinkSpeed = 2, rndSpeed = 7.0f;
    public float heightIncrease = 2.0f;
    Rigidbody rb;
    public bool canMove;
    public bool touchedFinishLine;
    [SerializeField]
    Transform spawnPoint, firstSp, secondSp, thirdSp, fourthSp;
    [SerializeField]
    float force = 3f;
    bool allow;
    
    [SerializeField]
    float waitForSecond = 3.0f;
    bool isCalled;
    void Start()
    {
        canMove = true;
        allow = true;
        isCalled = false;
        agent = GetComponent<NavMeshAgent>();
        enmyAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentIndex = 0;
        Invoke("SetDestination", 5f);
        spawnPoint = firstSp;

    }
    private void Update()
    {
        if (HY_StartPause.countOver)
        {
            if (canMove)
            {
                enmyAnim.SetFloat("Run", agent.velocity.sqrMagnitude);
                if (agent.isOnOffMeshLink)
                {
                    agent.speed = onLinkSpeed;

                    Vector3 targetPosition = agent.transform.position;
                    targetPosition.y += heightIncrease;

                    agent.transform.position = targetPosition;

                    enmyAnim.ResetTrigger("Dashing");
                    enmyAnim.SetBool("Dash", false);
                    //enmyAnim.SetBool("Jump", true);
                    enmyAnim.SetBool("Hanging", true);
                }
                else
                {
                    if (allow)
                    {
                        agent.speed = rndSpeed;
                    }
                    enmyAnim.SetBool("Jump", false);
                    enmyAnim.SetBool("Hanging", false);
                }
               
            }
            else
            {
                agent.speed = 0;
            }

            //if (transform.position.y > -50&& !isCalled)
            //{
            //    transform.position = spawnPoint.position;
            //    isCalled = true;
            //    SetDestination();
            //}
        }
    }
    public void SetDestination()
    {
        agent.SetDestination(wayPonints[currentIndex].position);

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Slider")
        {
            enmyAnim.SetTrigger("Dashing");
            enmyAnim.SetBool("Dash", true);
            agent.speed = rndSpeed * 1.5f;
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "WayPoints")
        {
            if (currentIndex < wayPonints.Length - 1)
            {
                currentIndex++;
                print(currentIndex);
            }
            agent.SetDestination(wayPonints[currentIndex].position);
            if (currentIndex >= wayPonints.Length - 1)
            {
                currentIndex = wayPonints.Length - 1;
            }
        }
        switch (other.tag)
        {
            case "FirstSp":
                spawnPoint = firstSp;
                break;
            case "SecondSp":
                spawnPoint = secondSp;
                break;
            case "ThirdSp":
                spawnPoint = thirdSp;
                break;
            case "FourthSp":
                spawnPoint = fourthSp;
                break;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collide" + gameObject.name);
            //agent.enabled = false;
            allow = false;
          GetComponent<NavMeshAgent>().enabled = false;
            rb.isKinematic = false;
            GetComponentInChildren<HY_EnemyRagdoll>().EnemyRagdoll();
            rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
            StartCoroutine(ResetPosition());
        }
        if (collision.transform.CompareTag("Ground"))
        {
            enmyAnim.SetBool("Hanging", false);
        }
    }
    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(waitForSecond);
        transform.position=spawnPoint.position;
        GetComponent<NavMeshAgent>().enabled = true;
        allow = true;
        rb.isKinematic = true;
        SetDestination();

    }




    //---------------------------------------------------------------------------------\\
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
