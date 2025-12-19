using UnityEngine;

public class StoneFloating : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float amplitude = 45f;

    private float initialZRotation,initialYPosition,initialZPosition, initialXPosition;

    private void Start()
    {

        initialZRotation = transform.eulerAngles.z;
        initialYPosition = transform.position.y;
        initialXPosition = transform.position.x;
        initialZPosition = transform.position.z;
        
    }

    private void Update()
    {

        float angle = Mathf.Sin(Time.time * speed) * amplitude;

        transform.position = new Vector3(initialXPosition,transform.position.y+angle,initialZPosition);

    }
}
