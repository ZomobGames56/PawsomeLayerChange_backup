using UnityEngine;

public class ClampXRotation : MonoBehaviour
{
    [SerializeField] private float minX = -9f;
    [SerializeField] private float maxX = 38f;

    void LateUpdate()
    {
        Vector3 currentRotation = transform.localEulerAngles;

        // Convert Unity's 0–360 to -180 to 180
        float x = currentRotation.x;
        if (x > 180f) x -= 360f;

        // Clamp X
        x = Mathf.Clamp(x, minX, maxX);

        // Apply rotation (Y & Z forced to 0)
        transform.localEulerAngles = new Vector3(x, 0f, 0f);
    }
}

