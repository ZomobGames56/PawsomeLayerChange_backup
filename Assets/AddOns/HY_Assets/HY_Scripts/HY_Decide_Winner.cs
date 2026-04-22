using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
public class HY_Decide_Winner : MonoBehaviour
{
    bool isPlayerWin, isEnemyWin;
    public float time;
    [SerializeField]
    HY_Player_Control playerControl;
    [SerializeField]
    HY_NavMeshEnemy[] enemyRef;
    [SerializeField]
    GameObject qualified, eliminated,cloudeExit;
    bool isCalled = false;
    [SerializeField]
    TextMeshProUGUI winnerCountTxt;
    int count;
    [SerializeField]
    AudioClip winClip, looseClip;
    [SerializeField]
    GameObject playerModel,_Level_Object;
    [SerializeField]
    float timeToShowWinnerScreen = 2.5f;
    [SerializeField]
    float playerRot;
    int winnerCount;
    bool once;
    Rigidbody playerRb;
    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    GameObject victoryPos;
    [SerializeField]
    GameObject mainCamera, mainCanvas, game_DL, victoryBoxObj;
    [SerializeField]
    List<Transform> otherPositions = new List<Transform>();
    [SerializeField]
    List<ZLerpMove> insideBox = new List<ZLerpMove>();
    private void Awake()
    {
        once = false;
        if (playerControl == null)
        {
            playerControl = FindFirstObjectByType<HY_Player_Control>();
        }
        count = 0;
        winnerCount = 0;
        playerRb = playerControl.GetComponent<Rigidbody>();
        victoryBoxObj.SetActive(false);
    }

    void UpdateUI()
    {
        winnerCountTxt.text = winnerCount + "/1";
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Player Win //Active Win Screen.
            isPlayerWin = true;
            OnPlayerWin();
            playerControl.GetComponent<Animator>().SetTrigger("Victory");
            if (isCalled == false)
            {
                foreach (var item in enemyRef)
                {
                    item.GetComponent<Animator>().SetTrigger("Defeat");
                    item.GetComponent<NavMeshAgent>().enabled = false;
                    item.GetComponent<HY_NavMeshEnemy>().enabled = false;
                }
                isCalled = true;
            }

            HY_Player_Control.canControl = false;
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (totalPlayers.Contains(other.gameObject))
            {
                totalPlayers.Remove(other.gameObject);
            }
            //StartCoroutine(LevelSelectionScene());
            StartCoroutine(VictoryBox(rb));
        }
        if (other.tag == "Enemy")
        {
            //Player Loose //Active Loose Screen.
            isEnemyWin = true;
            OnEnemyWin();
            HY_Player_Control.canControl = false;
            playerControl.GetComponent<Animator>().SetTrigger("Defeat");
            playerRb.isKinematic = true;

            // other.GetComponent<Animator>().SetTrigger("Victory");
            other.gameObject.GetComponent<HY_NavMeshEnemy>().touchedFinishLine = true;
            other.gameObject.GetComponent<HY_NavMeshEnemy>().enabled = false;
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            other.gameObject.GetComponent<Animator>().SetTrigger("Victory");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.transform.rotation = Quaternion.Euler(0, 0, 0);  
            if (totalPlayers.Contains(other.gameObject))
            {
                totalPlayers.Remove(other.gameObject);
            }
            if (isCalled == false)
            {
                foreach (var item in enemyRef)
                {
                    if (item.touchedFinishLine)
                    {
                        item.GetComponent<Animator>().SetTrigger("Victory");
                    }
                    else
                    {
                        if (item.isActiveAndEnabled)
                        {
                            item.GetComponent<Animator>().SetTrigger("Defeat");
                            item.GetComponent<NavMeshAgent>().enabled =false;
                            item.GetComponent<HY_NavMeshEnemy>().enabled =false;
                            //item.canMove = false;
                        }
                    }
                    isCalled = true;
                }
            }
            //StartCoroutine(LevelSelectionScene());
            //rb.rotation = Quaternion.identity;
            StartCoroutine(VictoryBox(rb));
        }
    }
    
    public void OnPlayerWin()
    {
        if (once) return;

        once = true;
        winnerCount = 1;

        qualified.gameObject.SetActive(true);
        eliminated.gameObject.SetActive(false);

        playerRb.isKinematic = true;

        HY_AudioManager.instance.PlayAudioEffectOnce(winClip);

        UpdateUI();
    }

    public void OnEnemyWin()
    {
        if (once) return;

        once = true;
        winnerCount = 1;

        qualified.gameObject.SetActive(false);
        eliminated.gameObject.SetActive(true);

        HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);

        UpdateUI();
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
        //rb.rotation = Quaternion.Euler(0,0,0);
        mainCamera.SetActive(false);
        mainCanvas.SetActive(false);
        game_DL.SetActive(false);
        victoryBoxObj.SetActive(true);
        _Level_Object.SetActive(false);

        for (int i = 0; i < totalPlayers.Count && i < otherPositions.Count; i++)
        {
            totalPlayers[i].transform.SetPositionAndRotation(
                otherPositions[i].position,
                otherPositions[i].rotation
            );
        }
        yield return new WaitForSeconds(2f);
        playerRb.isKinematic = false;    
        foreach (ZLerpMove z in insideBox)
        {
            z.pushOn = true;
        }
        yield return new WaitForSeconds(2f);
        cloudeExit.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        var handle = Addressables.LoadSceneAsync("LevelSelection", LoadSceneMode.Single);
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

