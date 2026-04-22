using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
public class StoryScript : MonoBehaviour
{
    private static string vedioString = "vedioString";
    public VideoPlayer videoPlayer; // Assign in Inspector
    public GameObject button;
    void Start()
    {
       
        if(PlayerPrefs.HasKey(vedioString))
        {
            button.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt(vedioString, 1);
            button.SetActive(false);
        }

        // Make sure we have a reference
        if (videoPlayer != null)
        {
            // Subscribe to the event
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    // This will be called when the video finishes
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished!");
        SkipButton();
    }
    public void SkipButton()
    {
        LoadSceneAsync("MainMenu");
    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }

    IEnumerator Load(string sceneName)
    {
        var handle = Addressables.LoadSceneAsync(sceneName,LoadSceneMode.Single);

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

    //public void SkipButton()
    //{
    //    Addressables.LoadSceneAsync("Jungle", LoadSceneMode.Single);
    //}

    //IEnumerator IPO()
    //{
    //    yield return new WaitUntil(()=> FirebaseInit.isFireBaseReady);
    //    AnalyticsEvents.GameLoaded();
    //}
}
