using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenSceneScript : MonoBehaviour
{
    bool once;
    private void Start()
    {
        once = false;
    }
    private void Update()
    {
        if(!once)
        StartCoroutine(loadScene());
        once = true;
    }
    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
