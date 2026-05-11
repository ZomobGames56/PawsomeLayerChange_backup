using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HY_LevelBtnManager : MonoBehaviour
{

    [SerializeField]
    GameObject loadingScreen, cloudObj, enterCloudObj;//, playerModel;
    [SerializeField]
    Image slider;
    float progress;
    int levelIndex;
    [SerializeField]
    AudioClip bgMucis, clickClip;
    public static bool canShowAd = false;

    // Start is called before the first frame update
    private void Start()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(bgMucis);
        enterCloudObj.SetActive(true);
        if (HY_Unity_LevelPlay_Ads.IsAdInitialize && canShowAd)
        {
            HY_Unity_LevelPlay_Ads.instance.ShowInterstitialAd();
        }
    }
    public void LoadGameLevel(string levelName)
    {
        
        HY_AudioManager.instance.PlayAudioEffectOnce(bgMucis);
        StartCoroutine(LoadScene(levelName));
    }
    IEnumerator LoadScene(string sceneKey)
    {
        cloudObj.SetActive(true);
        yield return new WaitForSeconds(1.55f);
        var handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single);

        while (!handle.IsDone)
        {
            float percent = handle.PercentComplete;

            yield return null;
        }

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Scene load failed");
        }
    }

}
