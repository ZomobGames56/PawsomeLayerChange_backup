using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class DummyPlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    float h, v;
    [SerializeField]
    float speed = 5;
    Vector3 dir;
    Rigidbody rb;
    public bool rigbdyContol, transfromControl;
    bool jump;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.U))
        {
            transfromControl = false;
            rigbdyContol = true;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            transfromControl = true;
            rigbdyContol = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 14,ForceMode.Impulse);
        }
        quaternion rot=Quaternion.Euler(transform.rotation.x,transform.rotation.y,0);
        transform.rotation = rot;
    }
    private void FixedUpdate()
    {

        dir = transform.right * h + transform.forward * v;
        //dir = new Vector3(h, 0, v);
        if (transfromControl)
        {
            transform.position += dir * speed * Time.fixedDeltaTime;
        }
        // rigdbgy
        if (rigbdyContol)
        {
            rb.MovePosition(rb.position+dir * speed * Time.fixedDeltaTime);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("TestWall"))
        {
            Debug.Log("TestWall Collide");  
        }
    }
}
