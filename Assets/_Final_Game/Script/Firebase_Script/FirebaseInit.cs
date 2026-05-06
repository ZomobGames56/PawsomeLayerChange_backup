using System;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
public class FirebaseInit : MonoBehaviour
{
    FirebaseApp app;
    public static bool isFireBaseReady { get; private set; } = false;
    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                print("firebase is intialized");
                isFireBaseReady = true;
                
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                AnalyticsEvents.GameLoaded();
            }
            else
            {
                Debug.LogError(String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
}
