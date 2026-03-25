//using System.Collections;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class HY_PlayerRotation : MonoBehaviour
//{
//    [SerializeField]
//    FixedTouchField playerTouchField;
//    float currentX;
//    [SerializeField]
//    float senstivity;
//    public Rigidbody rb;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        currentX += playerTouchField.TouchDist.x * senstivity * Time.deltaTime;
//        //transform.LookAt(lookAt);
//    }

//    private void FixedUpdate()
//    {
//        if (playerTouchField.TouchDist.x > 20)
//        {
//            rb.AddTorque(-Vector3.up * currentX,ForceMode.Impulse);
//        }
//        if (playerTouchField.TouchDist.x < -20)
//        {
//            rb.AddTorque(Vector3.up * currentX, ForceMode.Impulse);

//        }
//    }

//}
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class HY_PlayerRotation : MonoBehaviour
//{
//    [SerializeField] private FixedTouchField playerTouchField;
//    [SerializeField] private float sensitivity = 8f;
//    [SerializeField] private float torqueStrength = 6f;

//    private Rigidbody rb;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.angularDamping = 2f; // helps smooth stopping
        
//    }

//    private void FixedUpdate()
//    {
//        float swipeX = playerTouchField.TouchDist.x;

//        // small dead zone to avoid jitter
//        if (Mathf.Abs(swipeX) < 5f)
//            return;

//        // RIGHT swipe → RIGHT rotation
//        // LEFT swipe → LEFT rotation
//        float torque = swipeX * sensitivity * torqueStrength * Time.fixedDeltaTime;

//        rb.AddTorque(-Vector3.up * torque, ForceMode.Acceleration);
//    }
//}
