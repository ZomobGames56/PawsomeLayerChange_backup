using System.Collections;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [Header("Player Relocate Point")]
    [SerializeField]
    Transform playerCheckPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            HY_Player_Control.canControl = false;
        }
    }


    
}
