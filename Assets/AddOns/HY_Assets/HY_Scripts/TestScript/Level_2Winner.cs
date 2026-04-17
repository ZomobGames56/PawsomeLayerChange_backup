using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class Level_2Winner : MonoBehaviour
{
    float time;
    [SerializeField]
    HY_Player_Control playerControl;
    [SerializeField]
    NavMeshWithWayPointsAI[] enemyRef;
    bool isPlayerWin, isEnemyWin, isCalled;
    [SerializeField]
    GameObject  looseBGImg,winnerBGImg;
    int winnerCount;
   [SerializeField] TextMeshProUGUI winnerCountTxt;
    [SerializeField]
    AudioClip winnerClip,looseClip;
    [SerializeField]
    GameObject playerModel;
    [SerializeField]
    float timeToShowWinnerScreen = 2.5f;
    [SerializeField]
    GameObject mainCanvas, mainCamera;
    [SerializeField]
    float playerRot;
    bool once;

  
    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    List<Transform> otherPositions = new List<Transform>();
    [SerializeField]
    List<ZLerpMove> insideBox = new List<ZLerpMove>();
    [SerializeField]
    GameObject victoryPos,victoryBoxObj,cloudExit;
    Rigidbody playerRb;

    void Start()
    {
        //winningScreen.SetActive(false);
        once = false;
        if (playerControl == null)
        {
            playerControl = FindFirstObjectByType<HY_Player_Control>();
        }
        winnerCount = 0;
        playerRb  = playerControl.GetComponent<Rigidbody>();   
        victoryBoxObj.SetActive(false);
    }
    void PlayerWon()
    {
        winnerBGImg.SetActive(true);
        HY_AudioManager.instance.PlayAudioEffectOnce(winnerClip);
       
        once = true;
    }

    void EnemyWon()
    {
        looseBGImg.SetActive(true);
        winnerCount = 1;
        HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);
        
        once = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Player Win //Active Win Screen.
            isPlayerWin = true;
            PlayerWon();
            playerControl.GetComponent<Animator>().SetTrigger("Victory");
            winnerCount = 1;
            playerControl.GetComponent<Rigidbody>().isKinematic = true;
            HY_Player_Control.canControl = false;
            if (isCalled == false)
            {
                foreach (var item in enemyRef)
                {
                    item.GetComponent<Animator>().SetTrigger("Defeat");
                    item.GetComponent<NavMeshAgent>().enabled = false;
                    item.GetComponent<NavMeshWithWayPointsAI>().enabled = false;
                    //item.GetComponentInChildren<HY_EnemyRagdoll>().enabled = false;
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
            playerControl.GetComponent<Animator>().SetTrigger("Defeat");
            HY_Player_Control.canControl = false;
            winnerCount = 1;
            // other.GetComponent<Animator>().SetTrigger("Victory");
            other.gameObject.GetComponent<NavMeshWithWayPointsAI>().touchedFinishLine = true;
            EnemyWon();
            if (isCalled == false)
            {
                foreach (var item in enemyRef)
                {
                    if (item.touchedFinishLine)
                    {
                        item.GetComponent<Animator>().SetTrigger("Victory");
                        item.GetComponent<NavMeshWithWayPointsAI>().agent.speed = 0;
                        item.GetComponent<NavMeshWithWayPointsAI>().canMove = false;
                        item.GetComponentInChildren<HY_EnemyRagdoll>().enabled = false;
                    }
                    else
                    {
                        if (item.isActiveAndEnabled)
                        {
                            item.GetComponent<Animator>().SetTrigger("Defeat");
                            item.GetComponent<NavMeshWithWayPointsAI>().agent.speed = 0;
                            item.canMove = false;
                        }
                    }
                    isCalled = true;
                }
            }

        }
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
        
        victoryBoxObj.SetActive(true);

        for (int i = 0; i < totalPlayers.Count && i < otherPositions.Count; i++)
        {

            totalPlayers[i].transform.SetPositionAndRotation(
                otherPositions[i].position,
                otherPositions[i].rotation
            );
            totalPlayers[i].gameObject.SetActive(true);
            totalPlayers[i].GetComponent<Animator>().SetTrigger("Defeat");
        }
        yield return new WaitForSeconds(2f);
        playerRb.isKinematic = false;
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
