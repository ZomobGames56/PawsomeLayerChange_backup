using UnityEngine;

public class testmovement : MonoBehaviour
{
    float h, v;
    [SerializeField]
    float speed = 10f;
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward*v;
        transform.position += move*speed*Time.deltaTime;
    }
    private void FixedUpdate()
    {
        Vector3 move = transform.right * h + transform.forward * v;
        transform.position += move * speed * Time.fixedDeltaTime;
    } 
}
