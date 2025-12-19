using UnityEngine;

public class HY_Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float speed = 10f;
    void Update()
    {
        if (HY_StartPause.countOver)
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
    }
}
