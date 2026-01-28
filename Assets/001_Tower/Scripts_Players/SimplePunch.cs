using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimplePunch : MonoBehaviour
{
    [Header("Punch Movement")]
    [SerializeField] private Vector3 localDirection = Vector3.forward;
    [SerializeField] private float distance = 0.25f;
    [SerializeField] private float outTime = 0.06f;
    [SerializeField] private float backTime = 0.12f;

    [Header("Punch Force")]
    [SerializeField] private float punchForce = 6f;
    [SerializeField] private float upwardForce = 0.3f;

    private Vector3 startLocalPos;
    private Vector3 targetLocalPos;

    private Vector3 lastWorldPos;
    private Vector3 punchVelocity;
    private Vector3 smoothVelocity;

    private bool goingOut = true;
    private bool hasHit;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = false;
    }

    private void Start()
    {
        startLocalPos = transform.localPosition;
        targetLocalPos = startLocalPos + localDirection.normalized * distance;

        lastWorldPos = transform.position;
    }

    private void FixedUpdate()
    {
        MovePunch();

        // Calculate real punch velocity AFTER movement
        Vector3 currentWorldPos = transform.position;
        punchVelocity = (currentWorldPos - lastWorldPos) / Time.fixedDeltaTime;
        lastWorldPos = currentWorldPos;
    }

    private void MovePunch()
    {
        float smoothTime = goingOut ? outTime : backTime;
        Vector3 target = goingOut ? targetLocalPos : startLocalPos;

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            target,
            ref smoothVelocity,
            smoothTime,
            Mathf.Infinity,
            Time.fixedDeltaTime
        );

        if (Vector3.Distance(transform.localPosition, target) < 0.001f)
        {
            if (!goingOut)
                hasHit = false;

            goingOut = !goingOut;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!goingOut) return;
        if (hasHit) return;
        Rigidbody rb = collision.rigidbody;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (rb == null) return;

        Vector3 punchDir = punchVelocity.normalized;
        punchDir.y += upwardForce;

        rb.AddForce(punchDir.normalized * punchForce, ForceMode.Impulse);
        hasHit = true;
    }
}
