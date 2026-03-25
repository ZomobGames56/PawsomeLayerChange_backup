using UnityEngine;
using UnityEngine.EventSystems;

public class HY_OnPointerDown : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
   PlayerControl player_Ref;
    void Awake()
    {
        if (player_Ref == null)
        {
            player_Ref = FindAnyObjectByType<PlayerControl>();
        }

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        player_Ref.MobileJump();     
        Debug.Log("Jump Function");
    }

    

   
}
