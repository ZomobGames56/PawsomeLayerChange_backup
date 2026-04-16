using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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
    GameObject qualified, eliminated;
    bool isCalled = false;
    [SerializeField]
    TextMeshProUGUI winnerCountTxt;
    int count;
    [SerializeField]
    AudioClip winClip, looseClip;
    [SerializeField]
    GameObject playerModel;
    [SerializeField]
    float timeToShowWinnerScreen = 2.5f;
    [SerializeField]
    float playerRot;
    int winnerCount;
    bool once;
    Rigidbody playerRb;
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
    }
    //void Update()
    //{
    //    if (isPlayerWin && !once)
    //    {
    //        once = true;
    //        winnerCount = 1;
    //        qualified.gameObject.SetActive(true);
    //        eliminated.gameObject.SetActive(false);
    //        playerRb.isKinematic = true;
    //        HY_AudioManager.instance.PlayAudioEffectOnce(winClip);
    //        //StartCoroutine(ShowWinnerScreen());
    //       // HY_WinnerShowCase.instance.isPlayerWon = true;
    //    }
    //    if (isEnemyWin && !once)
    //    {
    //        once = true;
    //        //StartCoroutine(ShowWinnerScreen());
    //        winnerCount = 1;
    //        HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);
    //        qualified.gameObject.SetActive(false);
    //        eliminated.gameObject.SetActive(true);
    //       // HY_WinnerShowCase.instance.isPlayerWon = false;
    //    }
    //    winnerCountTxt.text = (winnerCount + "/1").ToString();
    //}
    void UpdateUI()
    {
        winnerCountTxt.text = winnerCount + "/1";
    }
    IEnumerator LevelSelectionScene()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(6);
      
    }

    //IEnumerator ShowWinnerScreen()
    //{
    //    yield return new WaitForSeconds(timeToShowWinnerScreen);
    //   // playerModel.GetComponent<Animator>().enabled = false;
    //   playerModel.transform.SetParent(stoneModel.transform);

    //    // playerModel.transform.localPosition = new Vector3(0, 0.001f, 0);
    //    //playerModel.transform.localRotation = Quaternion.Euler(0, playerRot, 0);
    //    Vector3 playerFinalPos = stoneModel.transform.position;
    //    playerFinalPos.y += 0.3f;
    //    playerRb.position = playerFinalPos;
    //    playerRb.rotation = Quaternion.Euler(0, playerRot, 0);
    //    playerModel.GetComponent<Animator>().ResetTrigger("Victory");
    //    mainCamera.SetActive(false);
    //    mainCanvas.SetActive(false);
    //    //showWinnerScreenCanvas.SetActive(true);
    //    ShowWinnerScreenCamera.SetActive(true);
    //    WinnerShowCaseScriptObj.SetActive(true);
    //    if (isPlayerWin)
    //    {
    //        HY_WinnerShowCase.instance.isPlayerWon = true;
    //    }
    //    else if (isEnemyWin)
    //    {
    //        HY_WinnerShowCase.instance.isPlayerWon = false;
    //    }
    //}


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
                    //item.GetComponent<NavMeshAgent>().speed = 0;
                    item.rndSpeed = 0;
                    item.canMove = false;
                }
                isCalled = true;
            }

            HY_Player_Control.canControl = false;
            StartCoroutine(LevelSelectionScene());
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
            StartCoroutine(LevelSelectionScene());

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


}

