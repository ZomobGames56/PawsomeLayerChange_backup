using UnityEngine;
using UnityEngine.EventSystems;

public class TouchIPointer : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    public PlayerControl player;

    bool isHolding;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        player.AttackPress();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        player.AttackRelease();
    }

    void Update()
    {
        if (isHolding)
            player.AttackHold();
    }
}
