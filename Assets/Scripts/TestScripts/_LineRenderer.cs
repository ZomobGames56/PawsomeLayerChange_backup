using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class _LineRenderer : MonoBehaviour
{
    LineRenderer lr;

    [SerializeField] Transform target;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    void LateUpdate()
    {
        if (!target)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = true;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, target.position);
    }
}
