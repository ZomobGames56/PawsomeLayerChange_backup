using System.Collections;
using TMPro;
using UnityEngine;

public class WoodenRotation_AI : MonoBehaviour
{
    [SerializeField]
    Transform jumpTarget, preDefinedJMT, testTarget;
    [SerializeField]
    float upForce = 6f;
    [SerializeField]
    float forwardForce = 4f, moveForce = 5f;
    [SerializeField]
    Transform moveTarget;
    Rigidbody rb;
    bool isGrounded;
    bool canMoveTowardTarget;
    [SerializeField]
    float stoppingDis = 1f;
    bool once = true;
    int currentInx = 0;
    [SerializeField]
    GameObject dummyPanel;
    [SerializeField]
    TextMeshProUGUI looseText;
    [SerializeField] float rotationSpeed = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canMoveTowardTarget = true;
        currentInx = 0;

    }

    void Update()
    {

        //if (moveTarget != null && Vector3.Distance(transform.position, moveTarget.position) < stoppingDis)
        //{
        //    canMoveTowardTarget = false;
        //}
        if (moveTarget != null && canMoveTowardTarget == true && isGrounded)
            MoveTowards();
    }

    void Jump()
    {
        if (!isGrounded) return;

        Vector3 toward = (jumpTarget.position - transform.position).normalized;
        toward.y = 0;
        // Smooth rotation (Slerp)
        Quaternion targetRot = Quaternion.LookRotation(toward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        //movemnt
        Vector3 jumpVector = toward * forwardForce + Vector3.up * upForce;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(jumpVector, ForceMode.Impulse);

    }
    void MoveTowards()
    {
        //if (!isGrounded) return;

        //Vector3 dir = (moveTarget.position - transform.position).normalized;
        //dir.y = 0;
        //transform.LookAt(dir);
        //rb.linearVelocity = new Vector3(dir.x * moveForce, rb.linearVelocity.y, dir.z * moveForce);
        if (!isGrounded) return;

        Vector3 dir = moveTarget.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        // Smooth rotation (Slerp)
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,rotationSpeed * Time.deltaTime);
        // transform.rotation = Quaternion.Euler(0f, targetRot.eulerAngles.y, 0f);
        //Movement 
        rb.linearVelocity = new Vector3(
            dir.normalized.x * moveForce,
            rb.linearVelocity.y,
            dir.normalized.z * moveForce
        );

    }
    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpDetect" && once)
        {
            once = false;
            canMoveTowardTarget = false;
            print("triggered");
            if (other.transform.childCount > 0)
            {
                jumpTarget = other.transform.GetChild(0);
                Debug.Log($"Object Name {jumpTarget.gameObject.name}");
            }
            Jump();

            StartCoroutine(ChangeMoveTarget());
        }
        if (other.tag == "Goal")
        {
            looseText.text = "Loose";
            dummyPanel.SetActive(true);
            Time.timeScale = 0;
        }
        if (other.tag == "PointIdentifier")
        {
            //canMoveTowardTarget = true;
            canMoveTowardTarget = true;
            moveTarget = preDefinedJMT;
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
        moveTarget = testTarget;
        canMoveTowardTarget = true;
        once = true;

    }
}
