using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HY_NavMeshEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator enmyAnim;

    [Header("Movement")]
    public float rndSpeed = 7f;
    [SerializeField] float onSliderSpeed = 10f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpDuration = 0.5f;

    public bool canMove = true;
    public bool touchedFinishLine = false;
    public bool followPath = true;

    bool hasSetDestination = false;
    bool isJumping = false;

    void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!enmyAnim) enmyAnim = GetComponentInChildren<Animator>();

        agent.speed = rndSpeed;
    }

    void Update()
    {
        if (!HY_StartPause.countOver || !canMove || touchedFinishLine)
        {
            agent.isStopped = true;
            return;
        }

        HandleMovement();
        HandleAnimation();
        HandleOffMeshLink();
    }

    void HandleMovement()
    {
        if (followPath && !hasSetDestination)
        {
            agent.SetDestination(target.position);
            hasSetDestination = true;
        }

        agent.isStopped = false;
        agent.speed = rndSpeed;
    }

    void HandleAnimation()
    {
        enmyAnim.SetFloat("Run", agent.velocity.magnitude);
    }

    void HandleOffMeshLink()
    {
        if (agent.isOnOffMeshLink && !isJumping)
        {
            StartCoroutine(JumpAcrossLink());
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

        isJumping = false;
        //enmyAnim.SetFloat("Run", agent.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slider"))
        {
            StartCoroutine(HandleSlider());
        }

        if (other.CompareTag("Finish"))
        {
            touchedFinishLine = true;
            StopMovement();
        }
        if (other.CompareTag("EnemyPoint"))
        {
            jumpHeight = 7;
            jumpDuration = 1.35f;
            rndSpeed = Random.Range(4, 6);
            Debug.Log("Val Changed");
        }
        if (other.CompareTag("RestVal"))
        {
            jumpHeight = 2;
            jumpDuration = 0.5f;
            Debug.Log("Val Changed");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slider"))
        {
            enmyAnim.SetBool("Dash", false);
        }
    }
    IEnumerator HandleSlider()
    {
        enmyAnim.SetTrigger("Dashing");
        enmyAnim.SetBool("Dash", true);

        float originalSpeed = rndSpeed;
        agent.speed = onSliderSpeed;

        yield return new WaitForSeconds(0.5f);

       // enmyAnim.SetBool("Dash", false);
        agent.speed = originalSpeed;
    }

    public void StopMovement()
    {
        canMove = false;
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        canMove = true;
        touchedFinishLine = false;
        hasSetDestination = false;
    }
}