using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_p : MonoBehaviour
{
    //list of balls
    // deactive object list 
    //active list 
    private static ObjectPool_p instance;

    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    // spawn 10 objet and and set deactive
    [SerializeField]
    GameObject poolObject;
    [SerializeField]
    int poolCount = 10;
    // [SerializeField]
    
    List<GameObject> deactiveObj = new List<GameObject>();
   // [SerializeField]
    List<GameObject> activeObj = new List<GameObject>();
    GameObject startPoolObject;
    private void Start()
    {
        startPoolObject = new GameObject("PoolContainer");
        for (int i = 0; i < poolCount; i++)
        {
            GameObject go = Instantiate(poolObject, startPoolObject.transform);
            go.transform.position = Vector3.zero;
            go.SetActive(false);
            deactiveObj.Add(go);


        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = GetObject();
            obj.SetActive(true);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.forward * 30f, ForceMode.Force);

        }
    }
    // spawn from deactive pool list
    GameObject GetObject()
    {
        if (deactiveObj.Count > 0)
        {
            //Give the Object to user
            GameObject obj = deactiveObj[Random.Range(0, deactiveObj.Count)];
            deactiveObj.Remove(obj);
            activeObj.Add(obj);
            return obj;
        }
        GameObject go = Instantiate(poolObject, startPoolObject.transform);
        go.SetActive(true);
        activeObj.Add(go);
        return go;


    }

    public static void GetDeactivePool(GameObject obj)
    {
        instance.deactiveObj.Add(obj);
        instance.activeObj.Remove(obj);
        obj.transform.position = Vector3.zero;  obj.transform.rotation = Quaternion.identity; 
        obj.SetActive(false);
    }
}
