using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerKnockback : MonoBehaviour
{
    public float stunDuration = 0.35f;
    public float verticalPop = 0.4f;
    public float dragDuringStun = 6f;

    Rigidbody rb;
    bool isStunned;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockbackFromNormal(Vector3 normal, float hitPower)
    {
        if (isStunned) return;

        Vector3 dir = -normal;
        dir.y = 0f;
        dir.Normalize();

        Vector3 force = dir * hitPower;
        force.y = verticalPop;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);

        StartCoroutine(Stun());
    }


    IEnumerator Stun()
    {
        isStunned = true;

        float originalDrag = rb.linearDamping;
        rb.linearDamping = dragDuringStun;

        yield return new WaitForSeconds(stunDuration);

        rb.linearDamping = originalDrag;
        isStunned = false;
    }

    public bool IsStunned() => isStunned;
}
