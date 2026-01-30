using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RandomAIMover : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float directionChangeTime = 2f;
    public float idleTime = 0.5f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    Rigidbody rb;
    Vector3 moveDir;
    bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        StartCoroutine(ChangeDirectionRoutine());
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = rb.linearVelocity.y; // preserve gravity
        rb.linearVelocity = velocity;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            // Pick random direction on XZ plane
            moveDir = new Vector3(
                Random.Range(-1f, 1f),
                0,
                Random.Range(-1f, 1f)
            ).normalized;

            canMove = true;
            yield return new WaitForSeconds(directionChangeTime);

            // Idle
            canMove = false;
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            yield return new WaitForSeconds(idleTime);
        }
    }
}
