using System.Collections;
using TMPro;
using UnityEngine;

public class WheelRotation_AI : MonoBehaviour
{
    public Transform jumpTarget, jumpInfo;
    public float upForce = 6f;
    public float forwardForce = 4f, moveForce = 5f;
    public Transform moveTarget;
    Rigidbody rb;
   public bool isGrounded;
   public bool canMoveTowardTarget;
    [SerializeField]
    float stoppingDis = 1f;
   public bool once = true;
    int currentInx = 0;
    [SerializeField]
    GameObject dummyPanel;
    public TextMeshProUGUI looseText;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canMoveTowardTarget = true;
        currentInx = 0;

    }

    void Update()
    {

        if (moveTarget != null && Vector3.Distance(transform.position, moveTarget.position) < stoppingDis)
        {
            canMoveTowardTarget = false;
        }
        if (moveTarget != null && canMoveTowardTarget == true && isGrounded)
            MoveTowards();
    }

    void Jump()
    {
        if (!isGrounded) return;

        Vector3 toward = (jumpTarget.position - transform.position).normalized;
        toward.y = 0;

        Vector3 jumpVector = toward * forwardForce + Vector3.up * upForce;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(jumpVector, ForceMode.Impulse);

    }
    void MoveTowards()
    {
        if (!isGrounded) return;

        Vector3 dir = (moveTarget.position - transform.position).normalized;
        dir.y = 0;
        transform.LookAt(dir);
        rb.linearVelocity = new Vector3(dir.x * moveForce, rb.linearVelocity.y, dir.z * moveForce);

    }
    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpDetect" && once)
        {
            Jump();
            once = false;
            canMoveTowardTarget = false;
            if (other.transform.childCount > 0)
            {
                moveTarget = other.transform.GetChild(0);
            }
            //else
            //{
            //    moveTarget = null;
            //}
            //other.gameObject.SetActive(false);
            StartCoroutine(ChangeMoveTarget());
        }
        if (other.tag == "Goal")
        {
            looseText.text = "Loose";
            dummyPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    //bool JumpDetect()
    //{
    //    return Vector3.Distance(transform.position, jumpInfo.position) < 10f;
    //}

    IEnumerator ChangeMoveTarget()
    {
        yield return new WaitForSeconds(1f);
        // currentInx++;
        canMoveTowardTarget = true;
        once = true;

    }
}
