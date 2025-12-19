using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class VoiceOver
{
    public AudioClip voiceClip;
    public string voiceText;
    public Sprite backImage;
    public VoiceOver(AudioClip clip, string textFile, Sprite backImage)
    {
        voiceClip = clip;
        voiceText = textFile;
        this.backImage = backImage;
    }
}
public class StoryScript : MonoBehaviour
{
    private static string storyString = "StroyStringPref";
    public bool isSkipping;
    public Image backGround;
    private int currentClipIndex;
    public AudioSource mainAudioSource;
    public TextMeshProUGUI dialogueText;
   // public MainMenuScript script;
    public GameObject skipButton;
    public List<VoiceOver> voiceOvers = new List<VoiceOver>();
    public GameObject playerModel;
    public GameObject audioSourceObj;
    public void StartStory()
    {

        if(PlayerPrefs.HasKey(storyString))
        {
            skipButton.SetActive(true);
        }
        else
        {
            skipButton.SetActive(false);
            PlayerPrefs.SetInt(storyString, 0);
        }
            playerModel.SetActive(false);
        isSkipping = false;
        if (voiceOvers.Count > 0)
        {
            // Start playing the list of clips
            backGround.sprite = voiceOvers[0].backImage;
            dialogueText.text = voiceOvers[0].voiceText;
            StartCoroutine(PlayAudioSequentially());
        }
        else
        {
            Debug.LogWarning("No audio clips assigned in the list!");
        }
    }

    IEnumerator PlayAudioSequentially()
    {
        while (currentClipIndex < voiceOvers.Count)
        {
            // Check if skip was pressed
            if (isSkipping)
            {
                break; // Exit the loop immediately if skipping
            }

            // Set the current background and dialogue text
            backGround.sprite = voiceOvers[currentClipIndex].backImage;
            dialogueText.text = voiceOvers[currentClipIndex].voiceText;

            // Set and play the current clip on the AudioSource
            mainAudioSource.clip = voiceOvers[currentClipIndex].voiceClip;
            mainAudioSource.Play();

            // Wait until the current clip finishes, or stop if skip is pressed
            float clipLength = mainAudioSource.clip.length;
            float elapsed = 0f;

            // Wait in intervals to allow checking for skipping
            while (elapsed < clipLength && !isSkipping)
            {
                yield return null;
                elapsed += Time.deltaTime;
            }

            // Move to the next clip if not skipping
            if (!isSkipping)
            {
                currentClipIndex++;
            }
            print("voice over ends");
        }
        Stopfunction();
        audioSourceObj.SetActive(true);
        mainAudioSource.Stop();
       // script.StoryEnded();
       gameObject.SetActive(false);
       //Playermodel Active
       playerModel.SetActive(true);
        StopCoroutine(PlayAudioSequentially());
    }
    public void Stopfunction()
    {
        isSkipping = true;
        currentClipIndex = voiceOvers.Count;
    }
}
