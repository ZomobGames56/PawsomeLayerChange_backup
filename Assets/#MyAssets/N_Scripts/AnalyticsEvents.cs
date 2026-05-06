using Firebase.Analytics;
using UnityEngine;

public class AnalyticsEvents : MonoBehaviour
{

    public static bool gameStarted = false;
    public static void GameLoaded()
    {
        if(FirebaseInit.isFireBaseReady)
        {
            gameStarted = true;
            FirebaseAnalytics.LogEvent("session_start");
            Debug.Log("session_start Event Called");
        }
        else
        {
            Debug.Log("Failed to load");
            
        }
    }
    public static void GameStartEvent()
    {
        if (FirebaseInit.isFireBaseReady)
        {
            gameStarted = true;
            FirebaseAnalytics.LogEvent("Game_Start", new Parameter("GameStarted", gameStarted ? 1 : 0));
            Debug.Log("GameStartEvent Called");
        }
        else
        {
            Debug.Log("Failed to load");
            
        }
    }
    public static void StoryCalled()
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("story_watch");
            Debug.Log("Story Watch log event");

        }
        else
        {
            Debug.Log("Failed to Story Log");

        }
    }
    public static void LevelStartEvent(string level)
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("Level_Start", new Parameter("Level", level));
            Debug.Log("LevelStartEvent Called - " + level);
        }
        else
        {
            Debug.Log("Failed to load level Logevent");
            
        }
    }
    public static void LevelChangeEvent(string level)
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("Level_Change", new Parameter("Level", level));
            Debug.Log("LevelChangeEvent Called - " + level);
        }
        else
        {
            Debug.Log("Failed to load");
            
        }
    }
    public static void RewardedAdComplete()
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("Rewarded ad complete / Player Revive");
            Debug.Log("Rewarded Ad Completed");
        }
        else
        {
            Debug.Log("Failed to load");
            
        }
    }
    public static void RewardedAdFail()
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("Rewarded ad failed");
            Debug.Log("Rewarded Ad failed");
        }
        else
        {
            Debug.Log("Failed to load");
            
        }
    }
    public static void InterstitialAdComplete()
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("Interstitial ad complete");
            Debug.Log("Interstitial Ad Completed");
        }
        else
        {
            Debug.Log("Failed to load");
            
        }
    }
    public static void InterstitialAdFail()
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("Interstitial ad failed to load");
            Debug.Log("Interstitial Ad failed");
        }
        else
        {
            Debug.Log("Failed to load");
            
        }
    }
    public static void LevelUnlocked(string level)
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("Level_Unlocked", new Parameter("Level", level));
            // FirebaseAnalytics.LogEvent("Status", new Parameter("Status", status));
            Debug.Log("New Level Unlcoked Called - " + level);
        }
        else
        {
            Debug.Log("Failed to load");

        }
    }
    public static void CharacterUnlocked(string chara)
    {
        if (FirebaseInit.isFireBaseReady)
        {
            FirebaseAnalytics.LogEvent("New Character Unlocked", new Parameter("Level", chara));
            // FirebaseAnalytics.LogEvent("Status", new Parameter("Status", status));
            Debug.Log(" - " + chara);
        }
        else
        {
            Debug.Log("Failed to load");

        }
    }
    public static void GameOverEvent()
    {
        if (FirebaseInit.isFireBaseReady)
        {
            gameStarted = false;
            FirebaseAnalytics.LogEvent("GameOver", new Parameter("GameOver", gameStarted ? 1 : 0));
            Debug.Log("GameOverEvent Called");
        }
        else
        {
            Debug.Log("Failed to load");

        }
    }
//#else
/*    // Runs in Editor and other platforms
    private static AnalyticsEvents instance;

    public static AnalyticsEvents Instance
    {
        get { return instance; }
    }
    public static bool gameStarted = false;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void GameLoaded()
    {
            Debug.Log("GameLoad Event Called");
    }
    public static void GameStartEvent()
    {
        Debug.Log("GameStartEvent Called");
    }
    public static void LevelStartEvent(string level)
    {
            Debug.Log("LevelStartEvent Called - " + level);
    }
    public static void LevelChangeEvent(string level)
    {
            Debug.Log("LevelChangeEvent Called - " + level);
    }
    public static void RewardedAdComplete()
    {
            Debug.Log("Rewarded Ad Completed");
    }
    public static void RewardedAdFail()
    {
            Debug.Log("Rewarded Ad failed");
    }
    public static void InterstitialAdComplete()
    {
            Debug.Log("Interstitial Ad Completed");
    }
    public static void InterstitialAdFail()
    {
            Debug.Log("Interstitial Ad failed");
    }
    public static void LevelUnlocked(string level)
    {
            Debug.Log("New Level Unlcoked Called - " + level);
    }
    public static void CharacterUnlocked(string chara)
    {
            Debug.Log(" Unlocked " + chara);
    }
    public static void GameOverEvent()
    {
            Debug.Log("GameOverEvent Called");
    }*/
//#endif
}
