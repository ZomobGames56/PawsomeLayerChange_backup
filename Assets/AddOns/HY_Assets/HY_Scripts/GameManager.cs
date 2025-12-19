using System.Collections;
using UnityEngine;
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
    public static bool storyOnStartOnly=false;
    private void Start()
    {
        print(storyOnStartOnly);
        if(!storyOnStartOnly)
        {
            storyPanel.SetActive(true);
            storyScript.StartStory();
            storyOnStartOnly = true;

        }
        else
        {
            storyPanel.SetActive(false);
            playerModel.SetActive(true);
        }
        //levelIndex = Random.Range(1, 3);
        loadingScreen.SetActive(false);
        cloudCanvas.SetActive(false);
    }
    public void PlayBtn()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        cloudCanvas.SetActive(true);
        yield return new WaitForSeconds(3);

        AsyncOperation operation = SceneManager.LoadSceneAsync(6);
        while (!operation.isDone)
        {
            //loadingScreen.SetActive(true);
            cloudCanvas.SetActive(true);
            progress = Mathf.Clamp01(operation.progress / .9f);
            slider.fillAmount = progress;
            playerModel.SetActive(false);
            yield return null;
        }
    }
}
