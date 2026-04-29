using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HY_OnJumper : MonoBehaviour
{
    [SerializeField]
    float JumperForce = 7.0f;
    Rigidbody rb;
    Animator playerAnim;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Jumper")
        {
            JumperForce = 55f;
            rb.AddForce(Vector3.up * JumperForce,ForceMode.Impulse);
            //playerAnim.ResetTrigger("Dashing");
            //playerAnim.SetBool("Dash",false);
            //playerAnim.SetBool("Hanging",true);

        }
        if (collision.transform.tag == "JumperTrigger")
        {
            JumperForce = 30f;
            rb.AddForce(Vector3.up * JumperForce, ForceMode.Impulse);
            //playerAnim.ResetTrigger("Dashing");
            //playerAnim.SetBool("Dash", false);
            //playerAnim.SetBool("Hanging", true);
            Debug.Log("Print");
        }
    }
   

}
