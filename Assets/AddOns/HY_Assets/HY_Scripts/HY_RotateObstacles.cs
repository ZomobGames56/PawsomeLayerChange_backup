using UnityEngine;

public class HY_RotateObstacles : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 2;
    float finalSpeed;
    [SerializeField]
    float x, y, z;
    public bool canRotate = true;
    // Update is called once per frame
   
    void Update()
    {
        if (!canRotate) return;
        // transform.rotation *= Quaternion.Euler(0, rotSpeed,0);
        transform.rotation *= Quaternion.Euler(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);

        //transform.Rotate(new Vector3(x,y,z));
    }
}
