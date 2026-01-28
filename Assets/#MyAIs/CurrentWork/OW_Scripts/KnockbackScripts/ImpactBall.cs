using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ImpactBall : MonoBehaviour
{
    public float minImpactSpeed = 6.5f;     // below this → no hit
    public float maxImpactSpeed = 18f;      // cap
    public float maxKnockbackForce = 14f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.AddForce(Vector3.right*(700),ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Debug.Log("Got Player");
        float speed = rb.linearVelocity.magnitude;

        // ❌ Too slow → ignore
        if (speed < minImpactSpeed)
            return;

        Debug.Log("max Speed");
        PlayerKnockback knockback = collision.gameObject.GetComponent<PlayerKnockback>();
        if (knockback == null) return;
        Debug.Log("Knock back is not null");
        // Normalize speed to 0–1
        float t = Mathf.InverseLerp(minImpactSpeed, maxImpactSpeed, speed);

        // Scale force like Stumble Guys
        float force = Mathf.Lerp(1f, maxKnockbackForce, t);

        Vector3 hitNormal = collision.contacts[0].normal;
        knockback.ApplyKnockbackFromNormal(hitNormal, force);

    }
}
