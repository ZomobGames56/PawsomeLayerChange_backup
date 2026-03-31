using UnityEngine;

public class Learn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float transFwd;
    void Start()
    {
        Vector3 i =transform.position+transform.forward*(-10);
        Debug.Log(i);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
