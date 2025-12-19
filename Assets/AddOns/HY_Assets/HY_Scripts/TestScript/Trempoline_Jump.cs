using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trempoline_Jump : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    SkinnedMeshRenderer blendSpaces;
    private void OnCollisionStay(Collision collision)
    {
        blendSpaces.SetBlendShapeWeight(0, 50);
    }
    private void OnCollisionExit(Collision collision)
    {
        blendSpaces.SetBlendShapeWeight(0, 100);
    }
}
