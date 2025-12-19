using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HY_StartPause : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    //Sprite[] sprite;
    [SerializeField]
    GameObject[] images;
    [SerializeField]
    Image img;
    public GameObject Enemies, Panel;
    public static bool countOver;
    [SerializeField]
    AudioClip countDownSound, bgMusic;
    [SerializeField]
    GameObject clouds, pauseMenu, cloudForExit;


    void Start()
    {
        if (pauseMenu != null)
        {

            pauseMenu.SetActive(false);
        }
        countOver = false;
        StartCoroutine(StartCount());
        cloudForExit.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !clouds.activeInHierarchy)
        {
            clouds.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Resume()
    {
        Time.timeScale = 1;
        // clouds.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void Leave()
    {
        Time.timeScale = 1;

        cloudForExit.SetActive(true);
        StartCoroutine(CloudWait());
        //SceneManager.LoadScene(6);
    }
    IEnumerator StartCount()
    {

        clouds.SetActive(true);
        yield return new WaitForSeconds(1);
        HY_AudioManager.instance.PlayAudioEffectOnce(countDownSound);
        // img.sprite = sprite[2];
        DeactiveAll();
        images[0].SetActive(true);

        yield return new WaitForSeconds(1f);
        DeactiveAll();
        images[1].SetActive(true);
        // img.sprite = sprite[1];

        yield return new WaitForSeconds(1f);
        DeactiveAll();
        images[2].SetActive(true);
        //img.sprite = sprite[0];

        yield return new WaitForSeconds(1f);
        DeactiveAll();
        images[3].SetActive(true);
        // img.sprite = sprite[3];
        yield return new WaitForSeconds(1f);
        countOver = true;
        HY_AudioManager.instance.StartBackgroundMusic(bgMusic);
        Time.timeScale = 1;
        if (Enemies != null)
        {
            Enemies.SetActive(true);
        }
        img.gameObject.SetActive(false);
        Panel.SetActive(false);

    }
    IEnumerator CloudWait()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(6);
    }
    void DeactiveAll()
    {
        foreach (GameObject go in images)
        {
            go.SetActive(false);
        }
    }
}
