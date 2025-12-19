using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float timeInMinutes = 1f; // Set your countdown time in minutes here
    private float currentTime;
    public TextMeshProUGUI timerText; // Assign this in the inspector
    [SerializeField]
    GameObject WinnerImg, LooserImg;
    [SerializeField]
    AudioClip winClip, looseClip;
    bool once;
    [SerializeField]
    public bool playerWon, enemyWon;
    public int redTeamScore, blueTeamScroce;
   
    void Start()
    {
        once = false;
        currentTime = timeInMinutes * 60; // Convert minutes to seconds
        UpdateTimerText();
        playerWon = false;
        enemyWon=false;
    }

    void Update()
    {
        redTeamScore = RedGoalPostHit.Instance.redBallScore;
        blueTeamScroce=BlueGoalPost.Instance.blueBallScore;
        if (HY_StartPause.countOver)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime; // Decrease the time by the time since last frame
                UpdateTimerText();

            }
            if (currentTime <= 0)
            {
                
                // //  redTeamScore = GoalPostHit.Instance.redBallScore;
                ////   blueTeamScroce=GoalPostHit.Instance.blueBallScore;
                //   print("RedTeamScore" + redTeamScore + " " + "BlueTeamScore" + blueTeamScroce);
                // //  print(GoalPostHit.Instance.gameObject.name);
               if (redTeamScore < blueTeamScroce)
                {
                    playerWon = true;
                    enemyWon = false;
                }
               else if(blueTeamScroce < redTeamScore)
                {
                    enemyWon = true;
                    playerWon=false;
                }
               else if (blueTeamScroce == redTeamScore)
                {
                    currentTime = 0.5f * 60;
                }
                TimerEnded();
            }

        }
        if (playerWon)
        {
            WinnerImg.SetActive(true);

        }
        else if (enemyWon)
        {
            LooserImg.SetActive(true);
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60); // Get minutes
        int seconds = Mathf.FloorToInt(currentTime % 60); // Get seconds
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Format time
    }


    void TimerEnded()
    {
        timerText.text = "Time's up!";

        WinnerCheck();
    }

    void WinnerCheck()
    {
        if (playerWon &&!once)
        {
            //WinnerImg.SetActive(true);
            //LooserImg.SetActive(false);
            HY_AudioManager.instance.PlayAudioEffectOnce(winClip);
            playerWon = true;
            once = true;
            StartCoroutine(LoadScene());

        }
        if (enemyWon && !once)
        {
            HY_AudioManager.instance.PlayAudioEffectOnce(looseClip);
            //LooserImg.SetActive(true);
            //WinnerImg.SetActive(false);
            enemyWon = true;
            once = true;
            StartCoroutine(LoadScene());


        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(5f);
        //player sound 
        SceneManager.LoadScene(6);
    }


}
