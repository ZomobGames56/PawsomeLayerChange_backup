using UnityEngine;

public class BallCollide : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float minImpactForce = 12f, impactMultiplier = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hip"))
        {
            Debug.Log("Collide to hip");
            // float impactStrength = collision.impulse.magnitude;

            float impactforce_ = rb.linearVelocity.magnitude;
            if (impactforce_ < minImpactForce) // tweak value
                return;

            Rigidbody playerRb = collision.collider.GetComponent<Rigidbody>();

            if (playerRb == null) return;

            HY_Player_Control.canControl = false;
           

            Vector3 dir = collision.contacts[0].normal * -1f;

            dir.y = 0.75f;
            if (collision.collider.TryGetComponent<IHitAble>(out var dmg))
            {
                dmg.ApplyKnoackBackForce(dir, impactforce_, impactMultiplier);
            }



            //  HY_PlayerRagdollActive.instance.OnObstacleCollide();
            //HipUpForceAdd.instance.ApplyKnoackBackForce(dir, impactforce_, impactMultiplier);


        }
        if (collision.collider.CompareTag("EnemyHip"))
        {
            float impactForce = rb.linearVelocity.magnitude;
            if (impactForce < minImpactForce)
                return;
            Rigidbody enemyRB = collision.collider.GetComponent<Rigidbody>();
            if (enemyRB == null) return;

            Vector3 dir = collision.contacts[0].normal * -1f;

            dir.y = 0.3f;
            if (collision.collider.TryGetComponent<IHitAble>(out var dmg))
            {
                dmg.ApplyKnoackBackForce(dir,impactForce, impactMultiplier);
            }
           
        }
    }



}
