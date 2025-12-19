using UnityEngine;

public class Floating : MonoBehaviour
{
    // Start is called before the first frame update
   [SerializeField] float floatSpeed=2, floatRange=2;
 
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatRange, 0);

    }
}
