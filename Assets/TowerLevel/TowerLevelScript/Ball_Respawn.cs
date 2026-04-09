using UnityEngine;

public class Ball_Respawn : MonoBehaviour
{
    [SerializeField]
    Transform startPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Water")
        {
            transform.position = startPos.position;
        }
    }
}
