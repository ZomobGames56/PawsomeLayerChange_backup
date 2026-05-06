using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Messaging;

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif

public class Fcm : MonoBehaviour
{
    [SerializeField]
    private string explanationMessage =
        "Would you like to receive notifications for rewards, events, and updates?";

    public Action OnPermissionGranted;
    public Action OnPermissionDenied;

#if UNITY_ANDROID && !UNITY_EDITOR
    private const string AndroidPermission = "android.permission.POST_NOTIFICATIONS";
#endif

    async void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        await RequestAndroidPermission();
#elif UNITY_IOS && !UNITY_EDITOR
        await RequestIOSPermission();
#else
        Debug.Log("Notification permission skipped (Editor or unsupported platform)");
#endif
    }

    // =========================
    // ANDROID
    // =========================
#if UNITY_ANDROID && !UNITY_EDITOR
    private async Task RequestAndroidPermission()
    {
        if (Permission.HasUserAuthorizedPermission(AndroidPermission))
        {
            OnPermissionGranted?.Invoke();
            return;
        }

        bool proceed = await ShowExplanationDialog();

        if (!proceed)
        {
            OnPermissionDenied?.Invoke();
            return;
        }

        Permission.RequestUserPermission(AndroidPermission);

        await Task.Delay(1000);

        if (Permission.HasUserAuthorizedPermission(AndroidPermission))
            OnPermissionGranted?.Invoke();
        else
            OnPermissionDenied?.Invoke();
    }
#endif

    // =========================
    // iOS
    // =========================
#if UNITY_IOS && !UNITY_EDITOR
    private async Task RequestIOSPermission()
    {
        bool proceed = await ShowExplanationDialog();

        if (!proceed)
        {
            OnPermissionDenied?.Invoke();
            return;
        }

        try
        {
            await FirebaseMessaging.RequestPermissionAsync();
            Debug.Log("iOS notification permission requested");
            OnPermissionGranted?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError("iOS permission request failed: " + e);
            OnPermissionDenied?.Invoke();
        }
    }
#endif

    // =========================
    // COMMON
    // =========================
    private Task<bool> ShowExplanationDialog()
    {
        Debug.Log("Explanation: " + explanationMessage);

        return Task.Run(async () =>
        {
            await Task.Delay(1000);
            return true; // Replace with real UI logic
        });
    }

    void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;

        SubscribeToTopic();
    }

    void OnDestroy()
    {
        FirebaseMessaging.TokenReceived -= OnTokenReceived;
        FirebaseMessaging.MessageReceived -= OnMessageReceived;
    }

    void SubscribeToTopic()
    {
        FirebaseMessaging.SubscribeAsync("FusionPrix");
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Message received");

        if (e.Message.Notification != null)
        {
            Debug.Log("Title: " + e.Message.Notification.Title);
            Debug.Log("Body: " + e.Message.Notification.Body);
        }

        if (e.Message.Data != null)
        {
            foreach (var pair in e.Message.Data)
            {
                Debug.Log("Key: " + pair.Key + " Value: " + pair.Value);
            }
        }
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        Debug.Log("FCM Token: " + e.Token);
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    public void OpenAppNotificationSettings()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var intent = new AndroidJavaObject("android.content.Intent"))
        {
            intent.Call<AndroidJavaObject>("setAction", "android.settings.APP_NOTIFICATION_SETTINGS");
            intent.Call<AndroidJavaObject>("putExtra", "android.provider.extra.APP_PACKAGE", Application.identifier);
            activity.Call("startActivity", intent);
        }
    }
#endif
}