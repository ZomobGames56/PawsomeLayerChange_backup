using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

                other.GetComponent<Rigidbody>().isKinematic = true;
                HY_Player_Control.canControl = false;
                other.gameObject.SetActive(false);
                enemyDeathCount = -1;
                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                //player die
                looserBGImg.SetActive(true);
                HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);
                int rnd = Random.Range(0, totalPlayers.Count);
                print(rnd);
                Rigidbody rb = totalPlayers[rnd].GetComponent<Rigidbody>();
                rb.rotation = Quaternion.identity;
                if (totalPlayers.Contains(totalPlayers[rnd]))
                {
                    totalPlayers[rnd].GetComponent<HY_RayCastAi>().enabled = false;
                    totalPlayers[rnd].GetComponent<Animator>().SetTrigger("Victory");

                    totalPlayers.Remove(totalPlayers[rnd]);
                }
                    StartCoroutine(VictoryBox(rb));
                break;


            case "Enemy":

                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                other.gameObject.SetActive(false);
                enemyDeathCount++;
                count = enemyDeathCount;
                eliminationTxt.text = count + "/6".ToString();
                if(!totalPlayers.Contains(other.gameObject))
                {
                    totalPlayers.Add(other.gameObject);
                }

                if (count >= enemyList.Count)
                {
                    //player win
                    playerRef.GetComponent<Rigidbody>().isKinematic = true;
                    winnerBGImg.SetActive(true);
                    HY_AudioManager.instance.PlayAudioEffectOnce(winClip);
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
        //players repose
        //main camera false
        //canvas false
        // direaction light false
        rb.position = victoryPos.transform.position;
        rb.rotation = victoryPos.transform.rotation;
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
        SceneManager.LoadScene(6);
    }

}
