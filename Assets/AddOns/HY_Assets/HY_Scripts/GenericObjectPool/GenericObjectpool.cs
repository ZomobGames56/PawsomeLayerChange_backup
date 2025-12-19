using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectpool<T> : MonoBehaviour where T : PoolObject
{
    public T ObjectToPool;
    public int PoolCount = 10;
    [SerializeField]
    private List<T> poolObj;
    private void Start()
    {
        InitializePoolObj();
    }
    void InitializePoolObj()
    {
        if (poolObj == null)
        {
            poolObj = new List<T>();
            for (int i = 0; i < PoolCount; i++)
            {
                AddPoolObj();
                print("Add 1");
            }
        }
        
    }
    T AddPoolObj()
    {
        T obj = Instantiate(ObjectToPool);
        obj.gameObject.SetActive(false);
        poolObj.Add(obj);
        return obj;
    }

    public T GetPoolObject()
    {
        foreach (T obj in poolObj)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }
        return AddPoolObj();
    }
}
