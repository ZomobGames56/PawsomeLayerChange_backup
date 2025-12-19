using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class HY_ComingSoonUpdater : MonoBehaviour
{
    [SerializeField]
    GameObject[] icons;
    [SerializeField]
    GameObject playerModel,comingSoonPanel;
    [SerializeField]
    AudioClip clickClip;
    private void Start()
    {
        DeactiveAll();
        comingSoonPanel.SetActive(false);

    }
    public void ComingSoon(int index)
    {
        switch (index)
        {
            case 0://party
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
                DeactiveAll();
                playerModel.SetActive(false);
                comingSoonPanel.SetActive(true);
                icons[index].SetActive(true);
                break;
            case 1://shop
                playerModel.SetActive(false);
                HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

                DeactiveAll();
                comingSoonPanel.SetActive(true);
                icons[index].SetActive(true);
                break;
            case 2://friend
                playerModel.SetActive(false);
                HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

                DeactiveAll();
                comingSoonPanel.SetActive(true);
                icons[index].SetActive(true);
                break;
            case 3://Mission
                playerModel.SetActive(false);
                HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

                DeactiveAll();
                comingSoonPanel.SetActive(true);
                icons[index].SetActive(true);
                HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

                break;
            default:
                playerModel.SetActive(false);
                HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);

                DeactiveAll();
                comingSoonPanel.SetActive(true);
                break;



        }
    }
    void DeactiveAll()
    {
        foreach (var icon in icons)
        {
            icon.gameObject.SetActive(false);
        }
    }
    public void Cross()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(clickClip);
        comingSoonPanel.SetActive(false);
        playerModel.SetActive(true);
    }
}
