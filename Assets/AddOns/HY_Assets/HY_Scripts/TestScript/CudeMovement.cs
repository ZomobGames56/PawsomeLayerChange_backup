using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CudeMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float speed = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.z >= 15)
        {
            speed = -5;
        }
        if (transform.position.z <= 0)
        {
            speed = 5;

        }
        transform.position += Vector3.forward * speed * Time.deltaTime;

    }
}
