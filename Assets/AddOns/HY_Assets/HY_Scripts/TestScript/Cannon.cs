using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Cannon : MonoBehaviour
{
    public Transform firePoint;
    public float fireForce = 15f; 
    public float fireInterval = 1f; 

    private ObjectPooler objectPooler;
    [SerializeField]
    float destroyTime = 3f;
    [SerializeField]
    Animator animator;
    void Start()
    {
      animator=GetComponent<Animator>();
        objectPooler = FindObjectOfType<ObjectPooler>();
        InvokeRepeating("Shoot", .65f, fireInterval);
    }

    void Shoot()
    {
       
        GameObject bomb = objectPooler.GetPooledObject();
        //animator.Play("Cannon_Attack");
        if (bomb != null)
        {
           
            bomb.transform.position = firePoint.position;
            bomb.transform.rotation = firePoint.rotation;

          
            bomb.SetActive(true);

          
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero; 
            rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);

          
            StartCoroutine(DeactivateAfterTime(bomb, destroyTime));
        }
    }

    private IEnumerator DeactivateAfterTime(GameObject bomb, float delay)
    {
        yield return new WaitForSeconds(delay);
        bomb.SetActive(false); 
    }
}
