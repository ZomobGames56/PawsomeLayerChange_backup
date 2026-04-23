using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class HY_DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    public int enemyDeathCount;
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
    [SerializeField]
    GameObject victoryPos, cloudExit;
    [SerializeField]
    GameObject mainCamera, mainCanvas, game_DL, victoryBoxObj;
    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    List<Transform> otherPositions = new List<Transform>();
    [SerializeField]
    List<ZLerpMove> insideBox = new List<ZLerpMove>();
    bool collide = false;
    void Start()
    {
        enemyDeathCount = 0;
        victoryBoxObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":

                if (collide) return;

                collide = true;
                other.GetComponent<Rigidbody>().isKinematic = true;
                HY_Player_Control.canControl = false;
                other.gameObject.SetActive(false);
                enemyDeathCount = -1;
                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                //player die
                looserBGImg.SetActive(true);
                HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);

                int rnd = Random.Range(1, totalPlayers.Count);
                print(rnd);

                Rigidbody rb = totalPlayers[rnd].GetComponent<Rigidbody>();

                rb.isKinematic = true;
                if (totalPlayers.Contains(totalPlayers[rnd]))
                {
                    totalPlayers[rnd].GetComponent<HY_RayCastAi>().enabled = false;
                    totalPlayers[rnd].GetComponent<Animator>().SetTrigger("Victory");

                    totalPlayers[rnd].SetActive(true);
                    totalPlayers[rnd].transform.position = victoryPos.transform.position;
                   
                    totalPlayers.Remove(totalPlayers[rnd]);
                }
                rb.rotation = Quaternion.Euler(0, 0, 0);
                StartCoroutine(VictoryBox(rb));
                break;


            case "Enemy":

                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                other.gameObject.SetActive(false);
                print(other.name + "False");
                enemyDeathCount++;
                count = enemyDeathCount;
                eliminationTxt.text = count + "/6".ToString();
                //if (!totalPlayers.Contains(other.gameObject))
                //{
                //    totalPlayers.Remove(other.gameObject);
                //}

                if (count >= enemyList.Count)
                {
                    //player win
                    playerRef.GetComponent<Rigidbody>().isKinematic = true;
                    HY_Player_Control.canControl = false;
                    winnerBGImg.SetActive(true);
                    HY_AudioManager.instance.PlayAudioEffectOnce(winClip);
                    Debug.Log("Player Win");
                    if (totalPlayers.Contains(playerRef.gameObject))
                    {
                        totalPlayers.Remove(playerRef.gameObject);
                    }
                    StartCoroutine(VictoryBox(playerRef));

                }

                break;
        }

    }

    IEnumerator LevelSelectionScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(6);

    }
    IEnumerator VictoryBox(Rigidbody rb)
    {
        yield return new WaitForSeconds(3f);

        rb.position = victoryPos.transform.position;
        rb.rotation = victoryPos.transform.rotation;
        rb.rotation = Quaternion.Euler(0, 0, 0);
        rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        mainCamera.SetActive(false);
        mainCanvas.SetActive(false);
        game_DL.SetActive(false);
        victoryBoxObj.SetActive(true);

        for (int i = 0; i < totalPlayers.Count && i < otherPositions.Count; i++)
        {
            totalPlayers[i].transform.SetPositionAndRotation(
                otherPositions[i].position,
                otherPositions[i].rotation
            );
            totalPlayers[i].SetActive(true);
            totalPlayers[i].GetComponent<Animator>().SetTrigger("Defeat");
            if (totalPlayers[i].GetComponent<HY_RayCastAi>() != null)
            {
                totalPlayers[i].GetComponent<HY_RayCastAi>().enabled = false;
                totalPlayers[i].GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        playerRef.isKinematic = false;
        playerRef.GetComponent<Animator>().SetTrigger("Victory");
        yield return new WaitForSeconds(2f);
        foreach (ZLerpMove z in insideBox)
        {
            z.pushOn = true;
        }
        yield return new WaitForSeconds(2f);
        cloudExit.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        var handle = Addressables.LoadSceneAsync("LevelSelection",LoadSceneMode.Single);

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
