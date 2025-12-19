using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class OWN_Boat : MonoBehaviour
{
    [SerializeField]
    float speed = 10f,r_speed = 0.75f,torque=80f, leanDeg= 45f;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Quaternion rot = Quaternion.Euler(-20, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot,speed*Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
            {
                Quaternion leanRot = Quaternion.Euler(transform.rotation.x, 0, leanDeg);
                transform.rotation = Quaternion.Slerp(transform.rotation, leanRot, speed * Time.deltaTime);

                rb.AddTorque(Vector3.up * -torque, ForceMode.Acceleration);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Quaternion leanRot = Quaternion.Euler(transform.rotation.x, 0, -45);
                transform.rotation = Quaternion.Slerp(transform.rotation, leanRot, speed * Time.deltaTime);

                rb.AddTorque(Vector3.up * torque, ForceMode.Acceleration);
            }
        }
       
        Quaternion target = Quaternion.identity;

        transform.rotation = Quaternion.Slerp(transform.rotation,target,r_speed*Time.deltaTime);


    }
}
