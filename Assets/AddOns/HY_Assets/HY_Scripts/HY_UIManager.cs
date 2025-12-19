using UnityEngine;
public class HY_UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject exitPanel, shopPanel, missionPanel, playerModel, settingPanel,
        selectedDailyMissionBtn, unSelectedDailyMissionBtn,//Mission buttons
        selectedWeeklyMissionBtn, unSelectedWeeklyMissionBtn,//Weekly Buttons
        selectAchivementBtn, unSelectedAchivementBtn,//Achivement Buttons
        dailyMissionPanel, weeklyMissionPanel, achivementPanel,cloudForLevel;// Panel references
    [SerializeField]
    AudioClip bgMusic,clickClip;
    
    void Start()
    {
        exitPanel.SetActive(false);
        shopPanel.SetActive(false);
        missionPanel.SetActive(false);
       // HY_AudioManager.instance.StartBackgroundMusic(bgMusic);
        //cloudForLevel.SetActive(true);

    }
    private void OnEnable()
    {
        cloudForLevel.SetActive(true);

    }
    
    private void Update()
    {
        //if()
    }
    public void ExitCross()//
    {
        exitPanel.SetActive(true);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
    }
    public void ContinuePlayBtn()//
    {
        exitPanel.SetActive(false);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

    }
    public void ExitBtn()//
    {
        Application.Quit();
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

    }
    public void ShopBtn()//
    {
        shopPanel.SetActive(true);
        playerModel.SetActive(false);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

    }
    public void MissionBtn()//
    {
        missionPanel.SetActive(true);
        playerModel.SetActive(false);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

    }
    public void HomeBtn()//
    {
        shopPanel.SetActive(false);
        playerModel.SetActive(true);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

    }
    public void MissionExitBtn()//
    {
        missionPanel.SetActive(false);
        playerModel.SetActive(true);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);


    }
    public void DailyMissionBtn()
    {
        dailyMissionPanel.SetActive(true);
        weeklyMissionPanel.SetActive(false);
        achivementPanel.SetActive(false);
        //////////////////////////////////////////
        selectedDailyMissionBtn.SetActive(true);
        selectedWeeklyMissionBtn.SetActive(false);
        selectAchivementBtn.SetActive(false);
        unSelectedDailyMissionBtn.SetActive(false);
        unSelectedWeeklyMissionBtn.SetActive(true);
        unSelectedAchivementBtn.SetActive(true);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);



    }
    public void WeeklyMissionBtn()
    {
        dailyMissionPanel.SetActive(false);
        weeklyMissionPanel.SetActive(true);
        achivementPanel.SetActive(false);
        /////////////////////////////////////////
        selectedDailyMissionBtn.SetActive(false);
        selectedWeeklyMissionBtn.SetActive(true);
        selectAchivementBtn.SetActive(false);
        unSelectedDailyMissionBtn.SetActive(true);
        unSelectedWeeklyMissionBtn.SetActive(false);
        unSelectedAchivementBtn.SetActive(true);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

    }
    public void AchivementBtn()
    {
        dailyMissionPanel.SetActive(false);
        weeklyMissionPanel.SetActive(false);
        achivementPanel.SetActive(true);
        /////////////////////////////////////////
        selectedDailyMissionBtn.SetActive(false);
        selectedWeeklyMissionBtn.SetActive(false);
        selectAchivementBtn.SetActive(true);
        unSelectedDailyMissionBtn.SetActive(true);
        unSelectedWeeklyMissionBtn.SetActive(true);
        unSelectedAchivementBtn.SetActive(false);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

    }

    public void SettingBtn()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
        playerModel.SetActive(false);
        settingPanel.SetActive(true);


    }
    bool isAnyPanelOpen()
    {
        if (shopPanel.activeInHierarchy || missionPanel.activeInHierarchy)
        {
            return true;
        }
        return false;
    }
    public void SettingExit()
    {
        settingPanel.SetActive(false);
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);


        if (isAnyPanelOpen())
        {
            playerModel.SetActive(false);
        }
        else
        {
            playerModel.SetActive(true);
        }

    }
}
