using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFire : MonoBehaviour
{
    public Transform firePoint; // The position where cannonballs spawn
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            FireCannon();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireCannon()
    {
        // Get a cannonball from the pool
        PoolObject cannonBall = PoolObject.Instance.GetPoolObject();
        cannonBall.transform.position = firePoint.position;
        cannonBall.transform.rotation = firePoint.rotation;
       
    }
}
