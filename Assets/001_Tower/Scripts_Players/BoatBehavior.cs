////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

////public class BoatBehavior : MonoBehaviour
////{
////    [SerializeField]
////    float speed,turnSpeed,turnTilt,maxTilt, waveForce, waveHeight, waterLevel;
////    Rigidbody rb;

////    void Start() => rb = GetComponent<Rigidbody>();

////    void FixedUpdate()
////    {
////        // Add boat forward movement with speed control
////        float move = Mathf.Max(Input.GetAxis("Vertical"), 0);
////        // Enable turning only when moving, with smooth torque
////        float turn = move > 0 ? Input.GetAxis("Horizontal") : 0;
////        rb.AddForce(transform.forward * move * speed);
////        rb.AddTorque(Vector3.up * turn * turnSpeed * Time.fixedDeltaTime);

////        // Implement tilt based on turning input
////        float tiltFromTurn = -turn * turnTilt;

////        // Add side-to-side wave sway only when stationary
////        float waveTilt = move > 0 ? 0 : Mathf.Sin(Time.time) * waveForce;
////        rb.AddForceAtPosition(Vector3.up * waveTilt, transform.position + transform.right);
////        rb.AddForceAtPosition(Vector3.up * -waveTilt, transform.position - transform.right);

////        // Clamp and smooth tilt angle for realistic leaning
////        Vector3 currentRotation = transform.eulerAngles;
////        float targetZ = Mathf.Clamp(tiltFromTurn + waveTilt, -maxTilt, maxTilt);
////        currentRotation.z = Mathf.LerpAngle(currentRotation.z, targetZ, Time.fixedDeltaTime * 5f);

////        // Simulate up-down wave motion relative to water level
////        float waveUpDown = Mathf.Sin(Time.time * 1.5f) * waveHeight;
////        Vector3 pos = transform.position;
////        pos.y = waterLevel + waveUpDown;
////        transform.position = pos;

////        // Apply rotation only on Y and Z axes
////        transform.eulerAngles = new Vector3(0, currentRotation.y, currentRotation.z);
////    }


////}

//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class BoatBehavior : MonoBehaviour
//{
//    [SerializeField] float speed = 50f;
//    [SerializeField] float turnSpeed = 20f;
//    [SerializeField] float tiltStrength = 10f;
//    [SerializeField] float maxTilt = 15f;
//    [SerializeField] float waveForce = 2f;
//    [SerializeField] float waveFrequency = 1.5f;
//    [SerializeField] float waterLevel = 0f;

//    Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.centerOfMass = Vector3.down * 0.5f; // stability
//        rb.angularDrag = 2f;
//        rb.drag = 0.5f;
//    }

//    void FixedUpdate()
//    {
//        float move = Mathf.Max(Input.GetAxis("Vertical"), 0f);
//        float turn = move > 0f ? Input.GetAxis("Horizontal") : 0f;

//        // Forward force
//        rb.AddForce(transform.forward * move * speed,ForceMode.Acceleration);

//        // Turn torque (no deltaTime)
//        rb.AddTorque(Vector3.up * turn * turnSpeed);

//        // Tilt sideways while turning
//        float targetTilt = -turn * maxTilt;
//        float currentTilt = Mathf.DeltaAngle(0, transform.eulerAngles.z);
//        float tiltTorque = (targetTilt - currentTilt) * tiltStrength;
//        rb.AddTorque(transform.forward * tiltTorque);

//        // Simulate waves with gentle up force
//        float wave = Mathf.Sin(Time.time * waveFrequency) * waveForce;
//        Vector3 lift = Vector3.up * wave;
//        rb.AddForce(lift, ForceMode.Acceleration);

//        // Keep near water level
//        //Vector3 pos = rb.position;
//        //if (Mathf.Abs(pos.y - waterLevel) > 0.5f)
//        //{
//        //    float correction = (waterLevel - pos.y) * 2f;
//        //    rb.AddForce(Vector3.up * correction, ForceMode.Acceleration);
//        //}
//    }
//}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardForce = 200f;
    [SerializeField] private float maxSpeed = 10f;

    [Header("Steering")]
    [SerializeField] private float turnSpeed = 90f;         // degrees/sec at full speed
    [SerializeField] private float minTurnFactor = 0.25f;   // how much you can turn at low speed
    [SerializeField] private float angularSmoothing = 8f;   // higher = snaps faster to desired yaw

    [Header("Handling")]
    [SerializeField] private float lateralDamping = 4f;     // removes side-slip (higher = less slip)
    [SerializeField] private float tiltStrength = 50f;      // torque applied to lean into turns
    [SerializeField] private float maxTilt = 12f;           // max roll angle in degrees

    [Header("Waves / Float")]
    [SerializeField] private float waveForce = 1f;
    [SerializeField] private float waveFrequency = 1.2f;
    [SerializeField] private float waterLevel = 0f;
    [SerializeField] private float floatStiffness = 5f;     // how strongly it returns to water level

    private Rigidbody rb;
    private float desiredYaw;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        desiredYaw = transform.eulerAngles.y;

        // sensible defaults; tune these on the Rigidbody in the inspector too
        rb.centerOfMass = Vector3.down * 0.5f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 1.5f;

        // prevent flipping forward/back but allow roll for tilt
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        // INPUT
        float moveInput = Mathf.Max(Input.GetAxis("Vertical"), 0f); // forward only; change if you want reverse
        float steerInput = moveInput > 0.01f ? Input.GetAxis("Horizontal") : 0f;

        // --- FORWARD FORCE ---
        rb.AddForce(transform.forward * moveInput * forwardForce);

        // clamp horizontal speed
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            Vector3 clamped = horizontalVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(clamped.x, rb.linearVelocity.y, clamped.z);
        }

        // --- STEERING: integrate desired yaw, scale by forward speed ---
        float forwardVel = Vector3.Dot(rb.linearVelocity, transform.forward); // positive when moving forward
        float speedFactor = Mathf.Clamp01(Mathf.Abs(forwardVel) / maxSpeed);
        float turnFactor = Mathf.Lerp(minTurnFactor, 1f, speedFactor);

        float yawDelta = steerInput * turnSpeed * turnFactor * Time.fixedDeltaTime;
        desiredYaw += yawDelta;

        Quaternion targetRot = Quaternion.Euler(0f, desiredYaw, 0f);
        Quaternion newRot = Quaternion.Slerp(rb.rotation, targetRot, angularSmoothing * Time.fixedDeltaTime);
        rb.MoveRotation(newRot);

        // --- LATERAL DAMPING (removes side-slip for crisp turning) ---
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x = Mathf.Lerp(localVel.x, 0f, lateralDamping * Time.fixedDeltaTime);
        Vector3 newVel = transform.TransformDirection(localVel);
        newVel.y = rb.linearVelocity.y; // keep vertical velocity
        rb.linearVelocity = newVel;

        // --- TILT (roll) into turns ---
        float targetTilt = Mathf.Clamp(-steerInput * maxTilt * turnFactor, -maxTilt, maxTilt);
        float currentTilt = Mathf.DeltaAngle(0f, transform.eulerAngles.z);
        float tiltError = targetTilt - currentTilt;
        float tiltTorque = Mathf.Clamp(tiltError * tiltStrength, -tiltStrength * 5f, tiltStrength * 5f);
        rb.AddTorque(transform.forward * tiltTorque);

        // --- WAVES / FLOAT ---
        float wave = Mathf.Sin(Time.time * waveFrequency) * waveForce;
        float heightError = waterLevel - rb.position.y;
        rb.AddForce(Vector3.up * (wave + heightError * floatStiffness), ForceMode.Acceleration);
    }
}


