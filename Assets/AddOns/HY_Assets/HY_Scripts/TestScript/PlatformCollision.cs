using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    [SerializeField]
    Transform targetTransform;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("RedBall")|| collision.transform.CompareTag("BlueBall"))
        {
            collision.transform.position = targetTransform.position;
            collision.gameObject.GetComponent<Rigidbody>().linearVelocity =Vector3.zero;
        }
    }
}
