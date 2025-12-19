using UnityEngine;

public class AdvancedBallBehavior : MonoBehaviour
{

    [SerializeField] private float force = 10, AI_force = 5f, wallForce = 3.5f, ballUpForce = 5.0f,goalKeeperForce=7.0f;
    private Rigidbody rb;
    //[SerializeField]
    //private GameObject moveTarget;
    [SerializeField]
    Transform blueGoalPostTarget, redGoalPostTarget, centerTarget;
    [SerializeField]
    AudioClip ballCollide;
    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from this game object.");
        }
        rb.mass = 1f; // Mass of the ball
        rb.linearDamping = 0.1f; // Air resistance
        rb.angularDamping = 0.05f; // Rotational drag
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy")
            || collision.collider.CompareTag("TeamPlayer"))
        {
            HY_AudioManager.instance.PlayAudioEffectOnce(ballCollide);
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            rb.AddForce(collision.transform.forward * force, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("Wall"))
        {
            HY_AudioManager.instance.PlayAudioEffectOnce(ballCollide);

            rb.AddForce(Vector3.up * wallForce, ForceMode.Impulse);
            rb.AddForce(collision.transform.forward * wallForce, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("BluePlayer"))
        {
            HY_AudioManager.instance.PlayAudioEffectOnce(ballCollide);

            rb.AddForce(Vector3.up * ballUpForce, ForceMode.Impulse);
            Vector3 goalPostDir = (blueGoalPostTarget.position - transform.position).normalized;
            rb.AddForce(goalPostDir * AI_force, ForceMode.Impulse);
            Debug.Log("Blue Hit blue Ball" + collision.gameObject.name);

        }
        if (collision.collider.CompareTag("RedPlayer"))
        {
            HY_AudioManager.instance.PlayAudioEffectOnce(ballCollide);

            rb.AddForce(Vector3.up * ballUpForce, ForceMode.Impulse);
            Vector3 goalPostDir = (redGoalPostTarget.position - transform.position).normalized;
            rb.AddForce(goalPostDir * AI_force, ForceMode.Impulse);
        }
        if (collision.collider.CompareTag("GoalKeeper"))
        {
            HY_AudioManager.instance.PlayAudioEffectOnce(ballCollide);

            rb.AddForce(Vector3.up * ballUpForce, ForceMode.Impulse);
            Vector3 goalPostDir = (centerTarget.position - transform.position).normalized;
            rb.AddForce(goalPostDir * goalKeeperForce, ForceMode.Impulse);
        }
        //if (collision.collider.CompareTag("Enemy"))
        //{
        //    transform.position = Vector3.Lerp(transform.position, moveTarget.transform.position, 5);
        //}
    }
   
    

}
