using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 TouchDelta;

    Vector2 lastPos;
    bool pressed;

    [SerializeField] float minMove = 2f;   // pixels needed before movement registers
    int activePointerID = -1;
    public void OnPointerDown(PointerEventData eventData)
    {
        activePointerID = eventData.pointerId;
        print("PointerDown");
        pressed = true;
        lastPos = eventData.position;
        TouchDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!pressed || eventData.pointerId!=activePointerID) return;

        Vector2 newPos = eventData.position;
        Vector2 delta = newPos - lastPos;

        //// 👉 if finger didn't actually move → ignore
        if (delta.sqrMagnitude < minMove * minMove)
        {
            TouchDelta = Vector2.zero;
            return;
        }

        TouchDelta = delta;
        lastPos = newPos;
        print("Drag");

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.pointerId!=activePointerID) return;

        pressed = false;
        TouchDelta = Vector2.zero;
    }

    void LateUpdate()
    {
        // if holding but finger not moving → stop camera
        if (pressed)
            TouchDelta = Vector2.zero;
    }
}
