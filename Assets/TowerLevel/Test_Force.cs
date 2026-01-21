using UnityEngine;

public class Test_Force : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    [SerializeField]
    float tq_Speed;
    public Transform startPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        rb.AddTorque(Vector3.one * tq_Speed, ForceMode.Force);
    }

  
}
