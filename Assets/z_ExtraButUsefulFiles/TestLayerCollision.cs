using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLayerCollision : MonoBehaviour
{

    void Update()
    {
        Physics.IgnoreLayerCollision(3, 8, true);
    }
}
