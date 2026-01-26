using UnityEngine;

public class BallCollisionCheck : MonoBehaviour
{
    [SerializeField]
    Transform startPos;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            transform.position = startPos.position;
        }
    }
}
