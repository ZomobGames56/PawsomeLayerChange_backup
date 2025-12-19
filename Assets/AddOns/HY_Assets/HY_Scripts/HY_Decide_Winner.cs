using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class HY_Decide_Winner : MonoBehaviour
{
    // Start is called before the first frame update
   // public static HY_Decide_Winner instance;
    
    bool isPlayerWin, isEnemyWin;
    public float time;
    [SerializeField]
    HY_Player_Control playerControl;
    [SerializeField]
    HY_NavMeshEnemy[] enemyRef;
    [SerializeField]
    NavMeshWithWayPointsAI enemyRef_New;
    [SerializeField]
    GameObject levelEndPanel, winnerBGImg, qualified, eliminated;
    [SerializeField]
    TextMeshProUGUI winLooseTxt;
    bool isCalled = false;
    [SerializeField]
    TextMeshProUGUI winnerCountTxt;
    int count;
    [SerializeField]
    AudioClip winClip, looseClip;
    [SerializeField]
    GameObject playerModel, stoneModel;
    [SerializeField]
    float timeToShowWinnerScreen = 2.5f;
    [SerializeField]
    GameObject mainCanvas, mainCamera, ShowWinnerScreenCamera,WinnerShowCaseScriptObj;
    [SerializeField]
    float playerRot;
    int winnerCount;
    bool once;
    private void Awake()
    {

        once = false;
        if (playerControl == null)
        {
            playerControl = FindObjectOfType<HY_Player_Control>();
        }
        count = 0;
        winnerCount = 0;
        ShowWinnerScreenCamera.SetActive(false);
    }
    void Update()
    {
        if (isPlayerWin && !once)
        {
            once = true;
            winnerCount = 1;
            qualified.gameObject.SetActive(true);
            eliminated.gameObject.SetActive(false);
            HY_AudioManager.instance.PlayAudioEffectOnce(winClip);
            StartCoroutine(ShowWinnerScreen());
           // HY_WinnerShowCase.instance.isPlayerWon = true;
        }
        if (isEnemyWin && !once)
        {
            once = true;
            StartCoroutine(ShowWinnerScreen());
            winnerCount = 1;
            HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);
            qualified.gameObject.SetActive(false);
            eliminated.gameObject.SetActive(true);
           // HY_WinnerShowCase.instance.isPlayerWon = false;
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
        if (isPlayerWin)
        {
            HY_WinnerShowCase.instance.isPlayerWon = true;
        }
        else if (isEnemyWin)
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
            if (isCalled == false)
            {
                foreach (var item in enemyRef)
                {
                    //item.GetComponent<NavMeshAgent>().speed = 0;
                    item.rndSpeed = 0;
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
            // other.GetComponent<Animator>().SetTrigger("Victory");
            other.gameObject.GetComponent<HY_NavMeshEnemy>().touchedFinishLine = true;
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
                            item.GetComponent<NavMeshAgent>().speed = 0;
                            item.canMove = false;
                        }
                    }
                    isCalled = true;
                }
            }

        }
    }


}

