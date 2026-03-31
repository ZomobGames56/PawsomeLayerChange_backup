using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class RagdollForce : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField]
    float backForce, upForce;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 dir = transform.forward;
            dir.y += upForce;
            rb.AddForce(dir * (-backForce), ForceMode.Impulse);
            StartCoroutine(RagDollGo());
        }
    }

    IEnumerator RagDollGo()
    {
        yield return new WaitForSeconds(0.15f);
      //  HY_PlayerRagdollActive.instance.OnObstacleCollide();

    }

}
