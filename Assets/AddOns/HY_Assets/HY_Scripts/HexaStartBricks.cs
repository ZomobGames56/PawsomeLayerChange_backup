using Unity.VisualScripting;
using UnityEngine;

public class HexaStartBricks : MonoBehaviour
{
    private void Update()
    {
        if (HY_StartPause.countOver)
        {
            transform.localScale = Vector3.zero;

        }
    }
}
