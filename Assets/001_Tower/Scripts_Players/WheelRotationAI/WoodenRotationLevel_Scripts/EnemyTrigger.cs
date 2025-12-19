using UnityEngine;

public sealed class EnemyTrigger : MonoBehaviour
{
    [SerializeField] MoveTargetRepositioner moveTarget;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            moveTarget.Reposition();
        }
    }
}
