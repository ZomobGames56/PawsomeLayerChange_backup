using System;
using UnityEngine;

public class mn_GameManager : MonoBehaviour
{
    public static Action OnWinEvent;
    public static Action BallActive;
    public static void Won()
    {
        OnWinEvent?.Invoke();
    }
    public static void BallStart()
    {
        BallActive?.Invoke();
    }
}
