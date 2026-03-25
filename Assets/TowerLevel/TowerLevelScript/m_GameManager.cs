using System;
using UnityEngine;

public class mn_GameManager : MonoBehaviour
{

    public static Action OnWinEvent;
    public static void Won()
    {
        OnWinEvent?.Invoke();
    }
}
