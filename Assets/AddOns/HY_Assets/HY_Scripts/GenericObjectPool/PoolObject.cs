using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : GenericObjectpool<PoolObject>
{
    private static PoolObject instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }
    public static PoolObject Instance
    {
        get { return instance; }
    }
}