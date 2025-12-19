using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HY_OnJumper : MonoBehaviour
{
    [SerializeField]
    float JumperForce = 7.0f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Jumper")
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * JumperForce,
              ForceMode.Impulse);
            GetComponent<Animator>().SetBool("Hanging", true);
        }

    }
   

}
