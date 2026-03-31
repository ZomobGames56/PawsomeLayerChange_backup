using Unity.Mathematics;
using UnityEngine;

public class LateForwardCameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -6f);
    [Header("Rotation")]
    [SerializeField] private float cameraPitch = 15f, cameraYaw; 
    [Header("Smoothness")]
    [SerializeField] private float positionSmooth = 5f;
    [SerializeField] private float rotationSmooth = 5f;

    void LateUpdate()
    {
        if (!target) return;

        // Desired position based on target forward
        Vector3 desiredPosition =
            target.position +
            target.forward * offset.z +
            target.up * offset.y +
            target.right * offset.x;

        // Smooth position follow
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmooth * Time.deltaTime
        );
        Quaternion targetYaw = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
        Quaternion pitch = Quaternion.Euler(cameraPitch, cameraYaw, 0f);

        Quaternion desiredRotation = targetYaw * pitch;
        //quaternion.LookRotation(desiredPosition,Vector3.up);
        transform.LookAt(desiredPosition);
        //// Smooth rotation to look forward
        //Quaternion desiredRotation =
        //   Quaternion.LookRotation(target.forward, Vector3.up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmooth * Time.deltaTime
        );
    }
}
