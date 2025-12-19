using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform player;
    public float lookAtSpeed = 5f;
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    Rigidbody rb;
    Quaternion rot;
    private Animator animator;
    public float detectionDistance = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (HY_StartPause.countOver)
        {
            rot = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                transform.rotation = rot;
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);

                directionToPlayer.y = 0;
                Vector3 movement = directionToPlayer * moveSpeed * Time.deltaTime;
                transform.position += movement;

                // Set animation based on distance to player
                // Adjust the "Speed" parameter to control the animation
                animator.SetFloat("Run", Mathf.Clamp01(1 - distanceToPlayer / detectionRange));
            }
            else
            {
                // Default to idle animation if player is out of range
                animator.SetFloat("Run", 0f);
            }
            //if (Input.GetKeyUp(KeyCode.LeftControl))
            //{
            //    rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            //    // jump Animation
            //}
        }
    }
    void ChangeDirection()
    {
        // Randomly change direction
        float randomAngle = Random.Range(-90f, 90f);
        transform.Rotate(0, randomAngle, 0);
    }
}
