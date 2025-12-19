using System.Collections;
using UnityEngine;

public class DeactiveTime : MonoBehaviour
{
    // UnityAction ImDone;
    // ObjectPool_p pool;
    void OnEnable()
    {
        StartCoroutine(GetDisable());   
    }
    IEnumerator GetDisable()
    {
        yield return new WaitForSeconds(5f);
        // pool.GetComponent<ObjectPool_p>();
        ObjectPool_p.GetDeactivePool(gameObject);
        
        //gameObject.SetActive(false);
    }
}
