using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HY_DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    public static int enemyDeathCount;
    public int count;
    [SerializeField]
    GameObject effect;
    [SerializeField]
    TextMeshProUGUI eliminationTxt;
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();
    [SerializeField]
    Rigidbody playerRef;
    [SerializeField]
    GameObject winnerBGImg, looserBGImg;
    [SerializeField]
    AudioClip winClip, looseClip;
    void Start()
    {
        enemyDeathCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                //efect show
                //set Deactive
                //Eliminate Text Shown
                other.GetComponent<Rigidbody>().isKinematic = true;
                HY_Player_Control.canControl = false;
                other.gameObject.SetActive(false);
                enemyDeathCount = -1;
                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                //player die
                looserBGImg.SetActive(true);
                HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);
                StartCoroutine(LevelSelectionScene());
                break;
            case "Enemy":
                //effect show
                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                other.gameObject.SetActive(false);
                enemyDeathCount++;
                count = enemyDeathCount;
                eliminationTxt.text = count + "/6".ToString();
                if (count >= enemyList.Count)
                {
                    //player win
                    playerRef.GetComponent<Rigidbody>().isKinematic = true;
                    winnerBGImg.SetActive(true);
                    HY_AudioManager.instance.PlayAudioEffectOnce(winClip);
                    StartCoroutine(LevelSelectionScene());
                }

                break;
        }

    }

    IEnumerator LevelSelectionScene()
    {
        yield return new WaitForSeconds(3f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(6);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            // Update loading UI here (progress bar etc.)
            yield return null;
        }

        // Small delay if you want
        yield return new WaitForSeconds(0.5f);

        asyncLoad.allowSceneActivation = true;
    }


}
