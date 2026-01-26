using UnityEngine;
using System.Collections;

public class SimplePunch : MonoBehaviour
{
    [SerializeField] Vector3 direction = Vector3.forward;
    [SerializeField] float distance = 0.25f;

    [SerializeField] float speedOut = 20f;   // fast punch
    [SerializeField] float speedBack = 6f;   // slow return

    Vector3 startPos;
    Vector3 targetPos;
    bool goingOut = true;

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + direction.normalized * distance;
    }

    void Update()
    {
        if (goingOut)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetPos,
                speedOut * Time.deltaTime
            );

            if (Vector3.Distance(transform.localPosition, targetPos) < 0.001f)
                goingOut = false;
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                startPos,
                speedBack * Time.deltaTime
            );

            if (Vector3.Distance(transform.localPosition, startPos) < 0.001f)
                goingOut = true;
        }
    }
}
   