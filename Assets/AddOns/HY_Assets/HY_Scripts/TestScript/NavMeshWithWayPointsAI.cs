using System.Collections;
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
    Transform spawnPoint, firstSp, secondSp, thirdSp, fourthSp,fourth_AI;
    [SerializeField]
    float force = 3f;
    bool allow, once;

    [SerializeField]
    float waitForSecond = 3.0f;
    bool isCalled;

    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpDuration = 0.5f;
    Coroutine jumpCoroutine;
    bool thirdIsPassed = false;
    bool isJumping = false;
    
    void Start()
    {
        canMove = true;
        allow = true;
        isCalled = false;
        agent = GetComponent<NavMeshAgent>();
        enmyAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentIndex = 0;
        StartCoroutine(SetPosition());
        spawnPoint = firstSp;
        once = false;
    }
    private void Update()
    {
        if (HY_StartPause.countOver)
        {
            if (canMove)
            {
                enmyAnim.SetFloat("Run", agent.velocity.sqrMagnitude);
                //if (agent.isOnOffMeshLink)
                //{
                //    agent.speed = onLinkSpeed;

                //    Vector3 targetPosition = agent.transform.position;
                //    targetPosition.y += heightIncrease;

                //    agent.transform.position = targetPosition;

                //    enmyAnim.ResetTrigger("Dashing");
                //    enmyAnim.SetBool("Dash", false);
                //    //enmyAnim.SetBool("Jump", true);
                //    enmyAnim.SetBool("Hanging", true);
                //}
                if (agent.isOnOffMeshLink && !isJumping)
                {
                    jumpCoroutine = StartCoroutine(JumpAcrossLink());
                }
                else if (!isJumping)
                {
                    if (allow)
                    {
                        agent.speed = rndSpeed;
                    }

                    enmyAnim.SetBool("Jump", false);
                    enmyAnim.SetBool("Hanging", false);
                }

            }
            //else
            //{
            //    agent.speed = 0;
            //}

            //if (transform.position.y > -50&& !isCalled)
            //{
            //    transform.position = spawnPoint.position;
            //    isCalled = true;
            //    SetDestination();
            //}
        }
    }

    IEnumerator JumpAcrossLink()
    {
        isJumping = true;

        agent.isStopped = true;
        agent.updatePosition = false;

        OffMeshLinkData data = agent.currentOffMeshLinkData;

        Vector3 startPos = transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float time = 0;

        enmyAnim.SetBool("Jump", true);
        enmyAnim.SetBool("Hanging", true);

        while (time < jumpDuration)
        {
            if (!isJumping) yield break; // 👈 abort safely

            float t = time / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;

            transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * height;

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        enmyAnim.SetBool("Jump", false);
        enmyAnim.SetBool("Hanging", false);

        agent.CompleteOffMeshLink();
        agent.updatePosition = true;
        agent.isStopped = false;
        agent.ResetPath(); // force recalculation
        SetDestination();
        isJumping = false;
    }
    void AbortOffMeshLink()
    {
        if (!isJumping) return;

        isJumping = false;

        if (jumpCoroutine != null)
            StopCoroutine(jumpCoroutine);

        agent.updatePosition = true;
        agent.isStopped = false;

        if (agent.isOnOffMeshLink)
            agent.CompleteOffMeshLink(); // or ResetPath()

        enmyAnim.SetBool("Jump", false);
        enmyAnim.SetBool("Hanging", false);
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
                thirdIsPassed = true;
                break;
            case "FourthSp":
                spawnPoint = fourthSp;
                 break;
            case "Fourth_AI":
                spawnPoint = fourth_AI;
                break;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !once)
        {
            Debug.Log("Collide" + gameObject.name);
            once = true;
            AbortOffMeshLink(); // 👈 ADD THIS
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
        once = false;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPoint.position, out hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        agent.enabled = false;
        agent.Warp(spawnPoint.position);
        agent.enabled = true;
        enmyAnim.Rebind();
        enmyAnim.Update(0f);
        allow = true;
        rb.isKinematic = true;
        yield return new WaitForSeconds(1f);
        SetDestination();

    }

    IEnumerator SetPosition()
    {
        yield return new WaitUntil(() => HY_StartPause.countOver);
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
