using UnityEngine;

public class HY_RotateObstacles : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 2;
    float finalSpeed;
    [SerializeField]
    float x, y, z;
    // Update is called once per frame
    private void Start()
    {
    }
    void Update()
    {
        // transform.rotation *= Quaternion.Euler(0, rotSpeed,0);
        transform.rotation *= Quaternion.Euler(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);

        //transform.Rotate(new Vector3(x,y,z));
    }
}
