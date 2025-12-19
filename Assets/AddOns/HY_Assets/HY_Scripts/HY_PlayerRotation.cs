using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HY_PlayerRotation : MonoBehaviour
{
    [SerializeField]
    FixedTouchField playerTouchField;
    float currentX;
    [SerializeField]
    float senstivity;
    public Rigidbody rb;
   
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentX += playerTouchField.TouchDist.x * senstivity * Time.deltaTime;
        //transform.LookAt(lookAt);
    }
    [System.Obsolete]
    private void FixedUpdate()
    {
        if (playerTouchField.TouchDist.x > 20)
        {
            rb.AddTorque(-Vector3.up * currentX,ForceMode.Impulse);
        }
        if (playerTouchField.TouchDist.x < -20)
        {
            rb.AddTorque(Vector3.up * currentX, ForceMode.Impulse);

        }
    }
  
}
