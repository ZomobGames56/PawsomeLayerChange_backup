using UnityEngine;

public class HY_ImageMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed = 6/*x = 5*/;
    RectTransform rectTransform;
    [SerializeField] private GameObject pos;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        MoveRight();
        if (rectTransform.position.x <= -1400 / 5)
        {
            rectTransform.position = new Vector3(1770 / 2, rectTransform.position.y, rectTransform.position.z);
            // rectTransform.position = pos.transform.position;

        }
    }
    void MoveRight()
    {
        rectTransform.position += Vector3.right * (-speed) * Time.deltaTime;
    }
}