using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenSceneScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(loadScene());
    }
   
    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
