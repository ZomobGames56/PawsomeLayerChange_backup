using UnityEngine;

public class CameraObstructionHandler : MonoBehaviour
{
    // player transform
    [SerializeField]
    Transform player;
    [SerializeField]
    LayerMask layerMask;

    Camera cam;
    private void Start()
    {
        cam = Camera.main;    
    }

    private void Update()
    {
        Vector3 origin = cam.transform.position;
        Vector3 direction = (player.position - origin).normalized;
        float distance = Vector3.Distance(origin, player.position);
        
        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.Log("HIT: " + hit.collider.name);
            
        }
       
            Debug.DrawRay(origin, direction * distance, Color.cyan);
    }
}
