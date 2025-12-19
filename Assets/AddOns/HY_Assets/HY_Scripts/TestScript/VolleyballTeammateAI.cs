using UnityEngine;
using UnityEngine.Rendering;

public class VolleyballTeammateAI : MonoBehaviour
{
    public Transform ball;            // The ball the team is playing with
    public Transform homePosition;    // The position this AI should hold on the court
    public float moveSpeed = 5f;      // Speed of movement
    public float hitDistance = 2f;    // Distance from the ball to hit it
    public float attackRange = 5f;    // Range within which the AI will try to hit the ball
    public bool isAttacker;           // If true, this AI is the one to attack (hit the ball)
    private Rigidbody rb;
    [SerializeField]
    private float lookAtSpeed = 5f,upwardHitForce=6f,forwardHitForce=8f;
    [SerializeField]
    Transform opponentCout;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
        if (IsBallWithinRange())
        {
            //perform jump
            isAttacker = true;
            if (isAttacker)
            {
                MoveTowardsBall();

                //// If close enough to the ball, attempt to hit
                //if (IsBallClose())
                //{
                //    HitBall();
                //}
            }
            else
            {
                MoveToHomePosition();
            }
        }
        else
        {
            isAttacker=false;
            MoveToHomePosition();
        }
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //Vector3 direction = (ball.position - transform.position).normalized;
        //Quaternion targetRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
    }

    // Check if the ball is within a range for the AI to start moving toward it
    bool IsBallWithinRange()
    {
        return Vector3.Distance(ball.position, transform.position) <= attackRange;
    }

    // Check if the AI is close enough to hit the ball
    bool IsBallClose()
    {
        return Vector3.Distance(ball.position, transform.position) <= hitDistance;
    }

    // AI moves toward the ball to hit it
    void MoveTowardsBall()
    {
        Vector3 direction = (ball.position - transform.position).normalized;
        direction.y = 0; // Keep the AI on the ground
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    // AI hits the ball when close enough
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("VolleyBall"))
        {

            Vector3 hitDirection = (opponentCout.position - ball.position).normalized;
           
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            ballRb.AddForce(Vector3.up * upwardHitForce, ForceMode.Impulse);

            ballRb.AddForce(hitDirection * forwardHitForce, ForceMode.Impulse);
            Debug.Log(gameObject.name + " hits the ball!");
        }
        
    }




    // AI moves to its designated position on the court when not attacking
    void MoveToHomePosition()
    {
        Vector3 direction = (homePosition.position - transform.position).normalized;
        direction.y = 0; // Keep the AI on the ground
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

    }

    // For debugging: Draw lines to the ball and home position in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, ball.position);  // Draw line to the ball
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, homePosition.position);  // Draw line to home position
    }
}
