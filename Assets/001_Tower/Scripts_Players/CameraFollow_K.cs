//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.ShaderGraph.Drawing;
//using UnityEngine;

//public class CameraFollow_K : MonoBehaviour
//{
//    /// <summary>
//    /// Camera follow 
//    /// 1. Offest
//    /// 2. follow Speed 
//    /// 3. target == playerTransform
//    /// </summary>


//    [SerializeField]
//    Transform target;
//    [SerializeField]
//    float followSpeed = 10f;
//    [SerializeField]
//    Vector3 offset = new Vector3(0,8,-8);
//    [SerializeField]
//    Quaternion rotation;

//    /// <summary>
//    /// For mouse axis movement
//    /// </summary>
//    float xAxis, yAxis;
//    [SerializeField]
//    float sensitivity,minY=5,maxY=55;
//    [SerializeField]
//    float leanAngle;

//    private void Update()
//    {
//        xAxis += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
//        yAxis -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
//        yAxis = Mathf.Clamp(yAxis, minY, maxY);

//        //float v = Input.GetAxis("Horizontal");
//        //transform.rotation*=Quaternion.Euler(-v*leanAngle,0,0); 

//    }

//    private void LateUpdate()
//    {
//        // transform.rotation = rot;
//        rotation = Quaternion.Euler(yAxis, xAxis, 0);
//        transform.position = target.position + rotation * offset;
//        //Vector3 targetPos= target.position + rotation * offset;
//        //transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed*Time.deltaTime);
//        //transform.position = Vector3.Lerp()
//        //transform.position = Vector3.soo
//        rotation.z = 0;
//        transform.LookAt(target.position);
//    }


//}
using UnityEngine;

public class CameraFollow_K : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 8, -8);
    [SerializeField] float followSpeed = 10f;

    [Header("Mouse Orbit")]
    [SerializeField] float sensitivity = 120f;
    [SerializeField] float minY = 5f;
    [SerializeField] float maxY = 55f;

    [Header("Auto Lean")]
    [SerializeField] float maxLean = 8f;       // degrees of tilt
    [SerializeField] float leanSpeed = 3f;     // how fast it blends

    float xAxis, yAxis;
    float currentLean;                         // z tilt
    float lastMouseMoveTime;
    [SerializeField] float mouseIdleTime = 0.25f; // seconds before auto lean activates

    void Update()
    {
        // --- Mouse orbit ---
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.001f || Mathf.Abs(mouseY) > 0.001f)
        {
            lastMouseMoveTime = Time.time;
            xAxis += mouseX * sensitivity * Time.deltaTime;
            yAxis -= mouseY * sensitivity * Time.deltaTime;
            yAxis = Mathf.Clamp(yAxis, minY, maxY);
        }

        // --- Auto Lean only if mouse idle ---
        float horizontal = Input.GetAxis("Horizontal");
        float targetLean = 0f;

        if (Time.time - lastMouseMoveTime > mouseIdleTime)
        {
            // tilt opposite of movement for a "banking" feel
            targetLean = -horizontal * maxLean;
        }

        // Smoothly interpolate current lean
        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);
    }

    void LateUpdate()
    {
        Quaternion orbitRot = Quaternion.Euler(yAxis, xAxis, 0);
        Vector3 desiredPos = target.position + orbitRot * offset;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Apply lean as extra roll
        transform.rotation = orbitRot * Quaternion.Euler(0, 0, currentLean);
    }
}

