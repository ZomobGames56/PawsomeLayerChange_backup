using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI_JumpToTarget : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    [SerializeField]
    Transform target;
    [SerializeField]
    float upForce=5f, forwardForce=9f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 dir = (target.position-transform.position).normalized;
            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            rb.AddForce(dir*forwardForce,ForceMode.Impulse);
            Debug.Log("Called");
        }

    }
}
