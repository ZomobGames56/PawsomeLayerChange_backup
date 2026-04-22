using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject loadingScreen,playerModel;
    [SerializeField]
    Image slider;
    float progress;
    int levelIndex;
    [SerializeField]
    GameObject cloudCanvas, storyPanel;
    [SerializeField]
    StoryScript storyScript;
    [SerializeField]
    AudioClip bgMucis;
    private void Start()
    {
        loadingScreen.SetActive(false);
        cloudCanvas.SetActive(false);
    }
    public void PlayBtn(string levelName)
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(bgMucis);
        StartCoroutine(LoadScene(levelName));
    }
    
    IEnumerator LoadScene(string sceneKey)
    {
       cloudCanvas.SetActive(true);
        yield return new WaitForSeconds(1.55f);
        var handle = Addressables.LoadSceneAsync(sceneKey,LoadSceneMode.Single);

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
