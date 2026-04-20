using UnityEngine;

public class MakeChildOnhit : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            return;

        other.transform.SetParent(transform);
    }
}
