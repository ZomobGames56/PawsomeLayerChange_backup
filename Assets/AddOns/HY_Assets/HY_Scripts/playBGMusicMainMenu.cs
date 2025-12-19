using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playBGMusicMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    AudioClip bgMusic;
    [SerializeField]
    GameObject voicePanel;
    bool once;
    // Update is called once per frame
    private void Start()
    {
        once = false;
    }
    void Update()
    {
        if (!voicePanel.activeInHierarchy&& !once)
        {
            HY_AudioManager.instance.StartBackgroundMusic(bgMusic);
            once = true;
        }
    }
}
