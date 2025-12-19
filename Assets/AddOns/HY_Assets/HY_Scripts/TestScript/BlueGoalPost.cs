using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGoalPost : MonoBehaviour
{
    public static BlueGoalPost Instance;
    [SerializeField]
    Transform resetPosition;
    public int blueBallScore;
    [SerializeField]
    GameObject blueGoalPostVfx;
    [SerializeField]
    AudioClip goalAudio;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedBall" || other.tag == "BlueBall")
        {
            // Goal Effect
            Rigidbody ballRb = other.gameObject.GetComponent<Rigidbody>();
            ballRb.linearVelocity = Vector3.zero;
            other.transform.position = resetPosition.position;
            if (other.tag == "BlueBall" && gameObject.name == "BlueGoalPostTarget")
            {
                blueBallScore++;
                FootballLevelUIManager.Instance.blueTeamScoreTxt.text = blueBallScore.ToString();
                blueGoalPostVfx.SetActive(true);
                HY_AudioManager.instance.PlayAudioEffectOnce(goalAudio);
            }
        }

    }
}
