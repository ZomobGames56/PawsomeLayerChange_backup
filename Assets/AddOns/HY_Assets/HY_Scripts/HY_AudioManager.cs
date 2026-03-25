using UnityEngine;
using UnityEngine.UI;

public class HY_AudioManager : MonoBehaviour
{
    public static HY_AudioManager instance;

    [SerializeField]
    AudioSource bgAudioSource,forOnShotPlayAudioSource;
    [SerializeField]
    AudioClip[] bgClips;
     public Slider musicSlider,sfxSlider;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        int rndIndex=Random.Range(0, bgClips.Length);
      //  StartBackgroundMusic(bgClips[rndIndex]);
       // musicSlider.value = 0.1f;
       // sfxSlider.value = 1f;
    }

    public void StartBackgroundMusic(AudioClip BackgroundClip)
    {
        bgAudioSource.clip = BackgroundClip;
        bgAudioSource.Play();
    }
    public void PlayAudioEffectOnce(AudioClip effectClip)
    {

        forOnShotPlayAudioSource.PlayOneShot(effectClip);
    }
}
