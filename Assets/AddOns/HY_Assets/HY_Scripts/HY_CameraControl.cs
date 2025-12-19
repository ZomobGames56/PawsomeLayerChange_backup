using UnityEngine;

public class HY_CameraControl : MonoBehaviour
{
    [SerializeField]
    float dis = 3f, minY = -50f, maxY = 50,
        sensivity = 200f, speed, sensivityY = 60, windowSensi = 50, windowSensiY = 50;
    [SerializeField]
    Transform lookAt, player;
    Quaternion rot;
    float currentY = 0, currentX = 0;
    [SerializeField]
    Vector3 dir;
    float rayDistance = 2.0f;
    public FixedTouchField TouchField;
    public float rotationSpeed = 1f;
    [SerializeField]
    float xAxisValue, yAxisValue;

    private void Start()
    {
        //rot = Quaternion.Euler(xAxisValue, yAxisValue, 0);
        // transform.rotation=Quaternion.Euler(180,0,0);
        //dir = new Vector3(11, 3, 0.5f);
        currentX = transform.rotation.x;
        currentY = transform.rotation.y;
    }
    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {

            MouseRotation();
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer ||
             Application.platform == RuntimePlatform.WindowsEditor)
        {
            RotationWithMouseOnly();
        }
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
        // ObstacleAvoidance();
    }

    void ObstacleAvoidance()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
        {

            if (hit.collider.CompareTag("Ground"))
            {

                minY = transform.localRotation.x;
                // Debug.Log("Raycast called" + gameObject.name);                  
            }
        }
        else
        {
            Debug.DrawLine(transform.position, transform.forward, Color.green);
            //agent.isStopped = false;
        }
    }
    public void MouseRotation()
    {
        currentX += TouchField.TouchDist.x * sensivity * Time.deltaTime;
        currentY -= TouchField.TouchDist.y * sensivityY * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, minY, maxY);

        if (TouchField.TouchDist.x > 0.1 || TouchField.TouchDist.x < -0.1 ||
                     TouchField.TouchDist.y > 0.1 || TouchField.TouchDist.y < -0.1)
        {

            rot = Quaternion.Euler(currentY, currentX, 0);

        }

        dir = new Vector3(0, 0, -dis);
        transform.position = lookAt.position + rot * dir;
        transform.LookAt(lookAt.position);

    }
    void RotationWithMouseOnly()
    {
        currentX += Input.GetAxis("Mouse X") * windowSensi * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * windowSensiY * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        rot = Quaternion.Euler(currentY, currentX, 0);

        dir = new Vector3(0, 0, -dis);
        transform.position = lookAt.position + rot * dir;
        transform.LookAt(lookAt.position);
    }
}