using UnityEngine;

public class HipUpForceAdd : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody hipRB;
    [SerializeField]
    float force = 6f;
    void Start()
    {
        hipRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 dir = Vector3.up;
            dir.z+= 0.5f;
            hipRB.AddForce(dir*force, ForceMode.Impulse);
        }
    }
}
