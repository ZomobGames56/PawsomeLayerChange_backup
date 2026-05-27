using TMPro;
using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.UI;

public class HY_Unity_LevelPlay_Ads : MonoBehaviour
{

    public static HY_Unity_LevelPlay_Ads instance;

    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedVideoAd;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button showAdButton, rewardedAdBtn;

    bool enableAd = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        

        if (IsAdInitialize) return;

        
        LevelPlay.ValidateIntegration();
        LevelPlay.SetMetaData("is_test_suite", "enable");


        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        LevelPlay.Init(AdConfig.AppKey);

        //showAdButton.interactable = false; 
    }
    void EnableAds()
    {
        LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;

        rewardedVideoAd = new LevelPlayRewardedAd(AdConfig.RewardedVideoAdUnitId);

        // Register to Rewarded Video events
        rewardedVideoAd.OnAdLoaded += RewardedVideoOnLoadedEvent;
        rewardedVideoAd.OnAdLoadFailed += RewardedVideoOnAdLoadFailedEvent;
        rewardedVideoAd.OnAdDisplayed += RewardedVideoOnAdDisplayedEvent;
        rewardedVideoAd.OnAdDisplayFailed += RewardedVideoOnAdDisplayedFailedEvent;
        rewardedVideoAd.OnAdRewarded += RewardedVideoOnAdRewardedEvent;
        rewardedVideoAd.OnAdClicked += RewardedVideoOnAdClickedEvent;
        rewardedVideoAd.OnAdClosed += RewardedVideoOnAdClosedEvent;
        rewardedVideoAd.OnAdInfoChanged += RewardedVideoOnAdInfoChangedEvent;

        interstitialAd = new LevelPlayInterstitialAd(AdConfig.InterstitalAdUnitId);

        // Register to Interstitial events
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
    }

    public static bool IsAdInitialize => instance != null && instance.enableAd;

    private void ImpressionDataReadyEvent(LevelPlayImpressionData data)
    {
        Debug.Log($"Received ImpressionDataReadyEvent ToString(): {data}");
        Debug.Log($"Received ImpressionDataReadyEvent allData: {data.AllData}");
    }

    public void ShowInterstitialAd()
    {
        if (!enableAd || interstitialAd == null)
        {
            Debug.Log("Ads not initialized yet");
            return;
        }

        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("Ad not ready yet");
        }
    }


    public void LoadInterstitialAd()
    {
        if (!enableAd || interstitialAd == null)
        {
            Debug.Log("Ads not initialized yet");
            //text.text = "Ads not initialized yet";

            return;
        }
        interstitialAd.LoadAd();

    }
    public void ShowRewardedAd()
    {
        if (!enableAd || rewardedVideoAd == null)
        {
            Debug.Log("Ad not initialized yet");
            //text.text = "Ad not initialized yet";
            return;
        }

        if (rewardedVideoAd.IsAdReady())
        {
            rewardedVideoAd.ShowAd();
        }
        else
        {

            Debug.Log("Ad not ready yet");
            //text.text = "Ad not ready yet "+rewardedVideoAd.IsAdReady();
        }

    }
    public void LoadRewardVideoAd()
    {
        if (!enableAd || rewardedVideoAd == null)
        {
            Debug.Log("Ads not initialized yet");
            return;
        }
        rewardedVideoAd.LoadAd();
    }

    private void SdkInitializationCompletedEvent(LevelPlayConfiguration configuration)
    {
        Debug.Log("SDK Init Success");
        //text.text = "SDK Init Success";
        LevelPlay.LaunchTestSuite();

        EnableAds();
        enableAd = true;

        interstitialAd.LoadAd(); // ✅ safe now
        rewardedVideoAd.LoadAd();
    }

    private void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.Log($"Init Failed: {error}");
        //text.text = $"Init Failed: {error}";
    }



    private void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo info)
    {
        string log = $"Received InterstitialOnAdInfoChangedEvent With AdInfo: {info}";
        //text.text = log;
    }

    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Ad Loaded");
        //text.text = $"Ad Loaded: {adInfo.AdNetwork}";

        //showAdButton.interactable = true; // ✅ enable button
    }

    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log($"Load Failed: {error}");
        //text.text = $"Load Failed: {error}";
    }

    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Ad Displayed");
        //text.text = "Ad Displayed";

        //showAdButton.interactable = false; // ❌ disable while showing
    }

    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Ad Closed");
        //text.text = $"Ad Closed: {adInfo.AdNetwork}";


        //showAdButton.interactable = false;

        interstitialAd.LoadAd(); // 🔥 preload next ad
        HY_LevelBtnManager.canShowAd = false;
    }

    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Ad Clicked");
    }

    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        Debug.Log($"Display Failed: {error}");
        //text.text = $"Display Failed: {error}";
        //TextUpdate(error);
        interstitialAd.LoadAd(); // retry
    }
    void TextUpdate(LevelPlayAdError m_error)
    {
        if (this.text != null)
        {
            text.text = $"Display Failed: {m_error}";
        }
    }
    void RewardedVideoOnLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Received RewardedVideoOnLoadedEvent With AdInfo: {adInfo}");
        //text.text = $"[Harsh Test] Received RewardedVideoOnLoadedEvent With AdInfo: {adInfo}";
    }

    void RewardedVideoOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log($"Received RewardedVideoOnAdLoadFailedEvent With Error: {error}");
    }

    void RewardedVideoOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Received RewardedVideoOnAdDisplayedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdDisplayedFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        Debug.Log($"Received RewardedVideoOnAdDisplayedFailedEvent With AdInfo: {adInfo} and Error: {error}");
    }

    void RewardedVideoOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        Debug.Log($"Received RewardedVideoOnAdRewardedEvent With AdInfo: {adInfo} and Reward: {reward}");
    }

    void RewardedVideoOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Received RewardedVideoOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Received RewardedVideoOnAdClosedEvent With AdInfo: {adInfo}");
        rewardedVideoAd.LoadAd();
    }

    void RewardedVideoOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"Received RewardedVideoOnAdInfoChangedEvent With AdInfo {adInfo}");
    }
}