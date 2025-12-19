using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyBallScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    float upForce=7f,forwardFroce=2.5f,delayTime=5f;
    [SerializeField]
    Transform opponentCoutPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        StartCoroutine(BallStop(delayTime));
    }

  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            Vector3 ballDir = (opponentCoutPos.position - transform.position).normalized;
            rb.AddForce(ballDir * forwardFroce, ForceMode.Impulse);
        }
    }
    IEnumerator BallStop(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        rb.isKinematic = false;
    }
}
