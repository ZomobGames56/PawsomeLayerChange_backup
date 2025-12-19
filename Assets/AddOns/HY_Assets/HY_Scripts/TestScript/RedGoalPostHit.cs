using UnityEngine;
public class RedGoalPostHit : MonoBehaviour
{
    public static RedGoalPostHit Instance;
    [SerializeField]
    Transform resetPosition;
    public int redBallScore;
    [SerializeField]
    GameObject redGoalPostVfx;
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

            if (other.tag == "RedBall" && gameObject.name == "RedGoalPostTarget")
            {
                redBallScore++;
                FootballLevelUIManager.Instance.redTeamScoreTxt.text=redBallScore.ToString();
                redGoalPostVfx.SetActive(true);
                HY_AudioManager.instance.PlayAudioEffectOnce(goalAudio);
            }
           
        }

    }
}
