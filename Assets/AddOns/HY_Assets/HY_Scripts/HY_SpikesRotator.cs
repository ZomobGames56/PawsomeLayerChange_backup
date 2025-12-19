using UnityEngine;

public class HY_SpikesRotator : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotSpeed = 20;
    float time;
    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void FixedUpdate()
    {
        TransformRotation();
    }

    // Use Event instead of this.......
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HY_PlayerRagdollActive.instance.RagdollActivate();
            
            Debug.Log(collision.gameObject.name);
        }
        if (collision.gameObject.tag == "Enemy")
        {
           // HY_EnemyRagdoll.instance.EnemyRagdoll();
            collision.gameObject.GetComponentInChildren<HY_EnemyRagdoll>().EnemyRagdoll();
           
        }
    }
    [System.Obsolete]
    void TransformRotation()
    {
        transform.rotation *= Quaternion.EulerRotation(new Vector3(0,1*rotSpeed * Time.deltaTime,0));
    }
}
