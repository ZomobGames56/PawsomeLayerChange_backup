using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HY_MakeItChild : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            other.transform.SetParent(transform);
            //set movement with transform
            other.gameObject.GetComponent<HY_Player_Control>().rigidBodyControl = false;
            other.gameObject.GetComponent<HY_Player_Control>().transformControl = true;

            
            print("Trigger");
        }
        if (other.tag == "Enemy")
        {
            other.transform.SetParent(transform);
            Debug.Log("Enemy collide");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
            //set movement with rigidbody
            other.gameObject.GetComponent<HY_Player_Control>().rigidBodyControl = true;
            other.gameObject.GetComponent<HY_Player_Control>().transformControl = false;

            print("Exit Trigger");

        }
        if (other.tag == "Enemy")
        {
            other.transform.SetParent(null);
        }
    }
}
