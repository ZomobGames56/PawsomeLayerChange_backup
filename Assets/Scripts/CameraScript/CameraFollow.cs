using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform lookAt;
    [SerializeField] Vector3 offSet = new Vector3(0, 3, -6);

    [SerializeField] float sensitivity = 0.2f;
    [SerializeField] float minY = -40f;
    [SerializeField] float maxY = 70f;

    float currentX;
    float currentY;
    [SerializeField] float minDrag = 5f;   // pixels before camera starts moving
    [SerializeField] FixedTouchField touchField;
    private void Start()
    {
        currentY = 35;
    }
    void LateUpdate()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 delta = touchField.TouchDelta;

        currentX += delta.x * sensitivity;
        currentY -= delta.y * sensitivity;

        currentY = Mathf.Clamp(currentY, minY, maxY);

        Quaternion rot = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rot * offSet;
        transform.LookAt(lookAt.position);




    }
}
