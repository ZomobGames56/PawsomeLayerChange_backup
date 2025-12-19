using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class _CheckWinner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();
    bool canRunUpdate;
    [SerializeField]
    GameObject levelEndPanel, winnerBGImg, looserBGImg;
    [SerializeField]
    TextMeshProUGUI winLooseTxt;
    float time;
    bool playerWon, enemyWon;
    [SerializeField]
    GameObject ref_Player;
    [SerializeField]
    AudioClip winClip, looseClip;
    [SerializeField]
    GameObject playerModel, stoneModel;
    [SerializeField]
    float timeToShowWinnerScreen = 2.5f;
    [SerializeField]
    GameObject mainCanvas, mainCamera, ShowWinnerScreenCamera, WinnerShowCaseScriptObj;
    [SerializeField]
    float playerRot;
    bool once;
    void Start()
    {
        once = false;
        canRunUpdate = true;
        winnerBGImg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canRunUpdate)
        {
            if (enemyList.Count <= HY_DeathZone.enemyDeathCount)
            {
                Debug.Log("Player Winner");
                playerWon = true;
                //can avtive Panel
                ref_Player.GetComponent<Rigidbody>().isKinematic = true;
                canRunUpdate = false;
            }
            if (HY_DeathZone.enemyDeathCount < 0)
            {
                enemyWon = true;

            }
        }
        WhoWon();

    }
    IEnumerator ShowWinnerScreen()
    {
        yield return new WaitForSeconds(timeToShowWinnerScreen);
        // playerModel.GetComponent<Animator>().enabled = false;
        playerModel.transform.SetParent(stoneModel.transform);
        playerModel.transform.localPosition = new Vector3(0, 0.001f, 0);
        playerModel.transform.localRotation = Quaternion.EulerAngles(0, playerRot, 0);
        if (!playerModel.activeInHierarchy)
        {
            playerModel.SetActive(true);
        }
        playerModel.GetComponent<Animator>().ResetTrigger("Victory");
        mainCamera.SetActive(false);
        mainCanvas.SetActive(false);
        //showWinnerScreenCanvas.SetActive(true);
        ShowWinnerScreenCamera.SetActive(true);
        WinnerShowCaseScriptObj.SetActive(true);
        if (playerWon)
        {
            HY_WinnerShowCase.instance.isPlayerWon = true;
        }
        else if (enemyWon)
        {
            HY_WinnerShowCase.instance.isPlayerWon = false;
        }



    }
    void WhoWon()
    {
        if (playerWon && !once)
        {
            once = true;

            StartCoroutine(ShowWinnerScreen());
            winnerBGImg.SetActive(true);
            HY_AudioManager.instance.PlayAudioEffectOnce(winClip);

        }
        else if (enemyWon && !once)
        {
            once = true;

            // winLooseTxt.text = "ELIMINATED";
            StartCoroutine(ShowWinnerScreen());
            HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);

            looserBGImg.SetActive(true);


        }
    }
}
