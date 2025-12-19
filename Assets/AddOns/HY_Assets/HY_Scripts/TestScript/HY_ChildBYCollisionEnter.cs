using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HY_ChildBYCollisionEnter : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy"))
        {
            collision.transform.SetParent(transform);
            Debug.Log("enter Called");
            collision.gameObject.GetComponent<HY_Player_Control>().rigidBodyControl = false;
            collision.gameObject.GetComponent<HY_Player_Control>().transformControl = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy"))
        {
            collision.transform.SetParent(null);
            Debug.Log("Exit Called");
             collision.gameObject.GetComponent<HY_Player_Control>().rigidBodyControl = true;
            collision.gameObject.GetComponent<HY_Player_Control>().transformControl = false;
        }
    }
}
