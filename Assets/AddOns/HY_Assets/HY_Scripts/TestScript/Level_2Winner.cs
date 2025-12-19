using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;


public class Level_2Winner : MonoBehaviour
{
    float time;
    [SerializeField]
    HY_Player_Control playerControl;
    [SerializeField]
    NavMeshWithWayPointsAI[] enemyRef;

    bool isPlayerWin, isEnemyWin, isCalled;

    [SerializeField]
    TextMeshProUGUI winLooseTxt;

    [SerializeField]
    GameObject levelEndPanel, looseBGImg,winnerBGImg,winningScreen;
    int winnerCount;
   [SerializeField] TextMeshProUGUI winnerCountTxt;
    [SerializeField]
    AudioClip winnerClip,looseClip;
    [SerializeField]
    GameObject playerModel, stoneModel;
    [SerializeField]
    float timeToShowWinnerScreen = 2.5f;
    [SerializeField]
    GameObject mainCanvas, mainCamera, ShowWinnerScreenCamera, WinnerShowCaseScriptObj;
    [SerializeField]
    float playerRot;
    bool once;
   // bool currentWinstatus;
   // public bool isPlayerWon;
    void Start()
    {
        //winningScreen.SetActive(false);
        once = false;
        if (playerControl == null)
        {
            playerControl = FindObjectOfType<HY_Player_Control>();
        }
        winnerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerWin&&!once)
        {
            winnerBGImg.SetActive(true);
            StartCoroutine(ShowWinnerScreen());
            //HY_WinnerShowCase.instance.isPlayerWon=true;
            HY_AudioManager.instance.PlayAudioEffectOnce(winnerClip);
            once = true;

        }
        if (isEnemyWin&&!once)
        {
            looseBGImg.SetActive(true);
            winnerCount = 1;
            StartCoroutine(ShowWinnerScreen());
           // HY_WinnerShowCase.instance.isPlayerWon=false;
            HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);
            once = true;
        }
        winnerCountTxt.text = (winnerCount + "/1").ToString();
        
    }
   
    IEnumerator ShowWinnerScreen()
    {
        yield return new WaitForSeconds(timeToShowWinnerScreen);
        // playerModel.GetComponent<Animator>().enabled = false;
        playerModel.transform.SetParent(stoneModel.transform);
        playerModel.transform.localPosition = new Vector3(0, 0.001f, 0);
        playerModel.transform.localRotation = Quaternion.EulerAngles(0, playerRot, 0);
        playerModel.GetComponent<Animator>().ResetTrigger("Victory");
        mainCamera.SetActive(false);
        mainCanvas.SetActive(false);
        //showWinnerScreenCanvas.SetActive(true);
        ShowWinnerScreenCamera.SetActive(true);
        WinnerShowCaseScriptObj.SetActive(true);
        if(isPlayerWin)
        {
            HY_WinnerShowCase.instance.isPlayerWon = true;
        }
        else if(isEnemyWin)
        {
            HY_WinnerShowCase.instance.isPlayerWon = false;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Player Win //Active Win Screen.
            isPlayerWin = true;
            playerControl.GetComponent<Animator>().SetTrigger("Victory");
            winnerCount = 1;
            if (isCalled == false)
            {
                foreach (var item in enemyRef)
                {
                    item.GetComponent<NavMeshWithWayPointsAI>().agent.speed = 0;
                    item.GetComponent<Animator>().SetTrigger("Defeat");
                   // item.rndSpeed = 0;
                    item.canMove = false;
                }
                isCalled = true;
            }
            HY_Player_Control.canControl = false;



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
            if (isCalled == false)
            {
                foreach (var item in enemyRef)
                {
                    if (item.touchedFinishLine)
                    {
                        item.GetComponent<Animator>().SetTrigger("Victory");
                        item.GetComponent<NavMeshWithWayPointsAI>().agent.speed = 0;
                        item.GetComponent<NavMeshWithWayPointsAI>().canMove = false;
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
}
