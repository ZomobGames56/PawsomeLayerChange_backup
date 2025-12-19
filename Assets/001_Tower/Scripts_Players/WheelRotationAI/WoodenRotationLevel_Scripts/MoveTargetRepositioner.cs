//using UnityEngine;

//public sealed class MoveTargetRepositioner : MonoBehaviour
//{
//    [Header("Z Axis Range")]
//    [SerializeField] float minZ = -72f;
//    [SerializeField] float maxZ = -36f;
//    [SerializeField] float searchSpeed = 12f;

//    [Header("Ground Check")]
//    [SerializeField] float rayDistance = 10f;
//    [SerializeField] LayerMask groundLayer;

//    bool isSearching;

//    Vector3 pos;

//    void Update()
//    {
//        if (!isSearching) return;

//        // Move forward on Z while searching
//        pos = transform.position;
//        pos.z += searchSpeed * Time.deltaTime;

//        // Safety clamp
//        if (pos.z > maxZ)
//        {
//            StopSearch();
//            return;
//        }

//        transform.position = pos;

//        // Check ground
//        if (IsGrounded(out RaycastHit hit))
//        {
//            pos.y = hit.point.y;
//            transform.position = pos;
//            StopSearch();
//        }
//    }

//    // 🔥 Call when enemy is triggered
//    public void Reposition()
//    {
//        pos = transform.position;
//        pos.z = Random.Range(minZ, maxZ);
//        transform.position = pos;

//        //// Immediate ground check
//        //if (!IsGrounded(out _))
//        //{
//        //    isSearching = true;
//        //}
//    }

//    bool IsGrounded(out RaycastHit hit)
//    {
//        return Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundLayer);
//    }

//    void StopSearch()
//    {
//        isSearching = false;
//    }

//#if UNITY_EDITOR
//    void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.cyan;
//        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
//    }
//#endif
//}
using UnityEngine;

public sealed class MoveTargetRepositioner : MonoBehaviour
{
    [Header("Z Axis Range")]
    [SerializeField] float minZ = -72f;
    [SerializeField] float maxZ = -36f;
    [SerializeField] float searchSpeed = 12f;

    [Header("Ground Check")]
    [SerializeField] float rayDistance = 15f;
    [SerializeField] float rayYOffset = 1.5f;
    [SerializeField] LayerMask groundLayer;

    bool isSearching;
    Vector3 pos;

    void Update()
    {
        if (!isSearching) return;

        // ✅ Check ground FIRST
        if (IsGrounded(out RaycastHit hit))
        {
            pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;

            StopSearch();
            return;
        }

        // ❌ Not grounded → move forward on Z
        pos = transform.position;
        pos.z += searchSpeed * Time.deltaTime;

        if (pos.z > maxZ)
        {
            StopSearch();
            return;
        }

        transform.position = pos;
    }

    // 🔥 Call when enemy triggers reposition
    public void Reposition()
    {
        pos = transform.position;
        pos.z = Random.Range(minZ, maxZ);
        transform.position = pos;

        isSearching = true; // ✅ REQUIRED
    }

    bool IsGrounded(out RaycastHit hit)
    {
        Vector3 origin = transform.position + Vector3.up * rayYOffset;
        return Physics.Raycast(origin, Vector3.down, out hit, rayDistance, groundLayer);
    }

    void StopSearch()
    {
        isSearching = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = transform.position + Vector3.up * rayYOffset;
        Gizmos.DrawRay(origin, Vector3.down * rayDistance);
    }
#endif
}

