using UnityEngine;

public class HipUpForceAdd : MonoBehaviour
{
    public static HipUpForceAdd instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody hipRB;
    [SerializeField]
    float force = 6f;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        hipRB = GetComponent<Rigidbody>();
    }
    public void ApplyKnoackBackForce(Vector3 knockBackDirection, float impactForce, float ImpactMultiplier)
    {
        hipRB.AddForce(knockBackDirection*impactForce*ImpactMultiplier, ForceMode.Impulse);
    }
}
