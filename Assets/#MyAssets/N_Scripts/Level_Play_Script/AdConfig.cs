public static class AdConfig
{
    public static string AppKey => GetAppKey();
    public static string BannerAdUnitId => GetBannerAdUnitId();
    public static string InterstitalAdUnitId => GetInterstitialAdUnitId();
    public static string RewardedVideoAdUnitId => GetRewardedVideoAdUnitId();

    static string GetAppKey()
    {
        #if UNITY_ANDROID
            return "264920d55";
#elif UNITY_IPHONE
            return "2649246dd";
#else
            return "unexpected_platform";
#endif
    }

    static string GetBannerAdUnitId()
    {
        #if UNITY_ANDROID
            return "thnfvcsog13bhn08";
        #elif UNITY_IPHONE
            return "iep3rxsyp9na3rw8";
        #else
            return "unexpected_platform";
        #endif
    }
    static string GetInterstitialAdUnitId()
    {
#if UNITY_ANDROID
            return "a7q52eiotj9uyi1t";
#elif UNITY_IPHONE
            return "4zun6iog5xnh8ke0";
#else
        return "unexpected_platform";
#endif
    }

    static string GetRewardedVideoAdUnitId()
    {
        #if UNITY_ANDROID
            return "ucovd5lo2hvzvi97";
#elif UNITY_IPHONE
            return "26e2axwy3frsbdap";
#else
            return "unexpected_platform";
#endif
    }
}
   