using UnityEngine;

public class CameraSimpleTransparency : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] Material transparentMat;

    Renderer currentRenderer;
    Material normalMat;
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Renderer hitRenderer = GetObstruction();

        if (hitRenderer != null)
        {
            if (currentRenderer != hitRenderer)
            {
                ResetMaterial();

                currentRenderer = hitRenderer;
                normalMat = currentRenderer.material;
                currentRenderer.material = transparentMat;
            }
        }
        else
        {
            ResetMaterial();
        }
    }

    Renderer GetObstruction()
    {
        Vector3 camPos = cam.transform.position;
        Vector3 dir = player.position - camPos;
        float dist = dir.magnitude;

        // 1️⃣ Raycast (camera outside object)
        if (Physics.Raycast(camPos, dir.normalized, out RaycastHit hit, dist, obstructionMask))
        {
            return hit.collider.GetComponent<Renderer>();
        }

        // 2️⃣ Overlap (camera inside object)
        Collider col = Physics.OverlapSphere(camPos, 0.15f, obstructionMask).Length > 0
            ? Physics.OverlapSphere(camPos, 0.15f, obstructionMask)[0]
            : null;

        return col ? col.GetComponent<Renderer>() : null;
    }

    void ResetMaterial()
    {
        if (currentRenderer != null)
        {
            currentRenderer.material = normalMat;
            currentRenderer = null;
        }
    }
}
