using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HY_LevelBtnManager : MonoBehaviour
{

    [SerializeField]
    GameObject loadingScreen,cloudObj;//, playerModel;
    [SerializeField]
    Image slider;
    float progress;
    int levelIndex;
    [SerializeField]
    AudioClip bgMucis, clickClip;
    // Start is called before the first frame update
    private void Start()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(bgMucis);

    }
    public void Level_1()
    {
        //cloudObj.SetActive(true);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
        StartCoroutine(LoadSceneAsync(2));
    }

    public void Level_2()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
       // cloudObj.SetActive(true);
        StartCoroutine(LoadSceneAsync(3));
    }

    public void Level_3()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
       // cloudObj.SetActive(true);
        StartCoroutine(LoadSceneAsync(4));
    }

    public void Level_4()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
       // cloudObj.SetActive(true);
        StartCoroutine(LoadSceneAsync(5));
    }
    public void LoadHome()
    {
        cloudObj.SetActive(true);
        StartCoroutine(LoadSceneAsync(1));
        //SceneManager.LoadScene(1);
    }
    IEnumerator LoadSceneAsync(int index)
    {
        cloudObj.SetActive(true);
        yield return new WaitForSeconds(3);
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            loadingScreen.SetActive(true);
            progress = Mathf.Clamp01(operation.progress / .9f);
            slider.fillAmount = progress;
            //playerModel.SetActive(false);
            yield return null;
        }
    }
}
