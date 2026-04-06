using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HY_OnJumper : MonoBehaviour
{
    [SerializeField]
    float JumperForce = 7.0f;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Jumper")
        {
            rb.AddForce(Vector3.up * JumperForce,ForceMode.Impulse);
            //GetComponent<Animator>().SetBool("Hanging", true);
        }
        if (collision.transform.tag == "JumperTrigger")
        {
            JumperForce = 30f;
            rb.AddForce(Vector3.up * JumperForce, ForceMode.Impulse);
            Debug.Log("Print");
        }
    }
   

}
