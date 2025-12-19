using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform ball;
    public Transform homePosition;
    public float moveSpeed = 5f;
    public float hitDistance = 2f;
    public float attackRange = 5f;
    public bool isAttacker;
    private Rigidbody rb;
    [SerializeField]
    private float lookAtSpeed = 5f, upwardHitForce = 6f, forwardHitForce = 8f;
    [SerializeField]
    Transform opponentCout;
    Animator animator;
    Vector3 b_direction;
    bool noMoving;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Slerp(transform.rotation, ball.rotation, lookAtSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation=Quaternion.Euler(0, transform.rotation.y, 0);
        transform.LookAt(ball.position);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if (IsBallWithinRange())
        {
            //perform jump
            if (isAttacker)
            {
                MoveTowardsBall();
            }
            else
            {
                MoveToHomePosition();
            }
        }
        else
        {
            MoveToHomePosition();

        }
        if (noMoving)
        {

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
           // Vector3 direction = (ball.position - transform.position).normalized;
           //Quaternion targetRotation = Quaternion.LookRotation(direction);
           // targetRotation.x = 0;
           // transform.rotation = targetRotation;
        }
    }

    void MoveTowardsBall()
    {
        b_direction = (ball.position - transform.position).normalized;
        b_direction.y = 0; // Keep the AI on the ground
        if (b_direction.magnitude != 0)
        {
            animator.SetFloat("Run", b_direction.magnitude);
        }
        else
        {
            animator.SetFloat("Run", 0);
        }
        rb.MovePosition(transform.position + b_direction * moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        Quaternion targetRotation = Quaternion.LookRotation(b_direction);
        targetRotation.x = 0;
        transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
    }

    bool IsBallWithinRange()
    {
        return Vector3.Distance(ball.position, transform.position) <= attackRange;
    }
   
    void MoveToHomePosition()
    {

        Vector3 direction = (homePosition.position - transform.position).normalized;
        direction.y = 0; // Keep the AI on the ground
        if (direction.magnitude != 0)
        {
            animator.SetFloat("Run", direction.magnitude);
            noMoving = true;
        }
        else
        {
            animator.SetFloat("Run", 0);
            noMoving = true;
        }
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        if (direction.magnitude != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation.x = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
        }
    }
}
