using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundScroller : MonoBehaviour
{

    [SerializeField] private float speedX = 0.1f;
    [SerializeField] private float speedY = 0f;

    private RawImage img;
    private Vector2 offset;
    Vector2 scale;
    void Start()
    {
        img = GetComponent<RawImage>();
        scale = img.uvRect.size;
    }

    void Update()
    {
        offset.x += speedX * Time.deltaTime;
        offset.y += speedY * Time.deltaTime;

        img.uvRect = new Rect(offset, scale);
    }
}
