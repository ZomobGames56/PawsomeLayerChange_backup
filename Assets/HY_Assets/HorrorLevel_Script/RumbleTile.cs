using UnityEngine;

public class RumbleTile : MonoBehaviour
{
    public Material normalMat;
    public Material warningMat;

    Rigidbody rb;
    MeshRenderer mr;

    public bool IsMarked { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();

        rb.isKinematic = true;
        rb.useGravity = false;
        mr.material = normalMat;
    }

    public void Mark()
    {
        if (IsMarked) return;

        IsMarked = true;
        mr.material = warningMat;
    }

    public void Fall()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void ResetTile()
    {
        IsMarked = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        mr.material = normalMat;
        //Back to the old position.
    }

   
}
