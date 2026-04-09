using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
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
        LoadSceneAsync("0_MainMenu_");
    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }

    IEnumerator Load(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            Debug.Log(op.progress); // 0 → 0.9
            yield return null;
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
