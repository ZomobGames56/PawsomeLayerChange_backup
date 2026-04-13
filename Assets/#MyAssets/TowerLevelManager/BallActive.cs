using UnityEngine;

public class BallActive : MonoBehaviour
{
    [SerializeField]
    GameObject b;
    private void Start()
    {
        b.SetActive(false);
    }
    private void OnEnable()
    {
        mn_GameManager.BallActive += ActiveBall;
    }
    private void OnDisable()
    {
        mn_GameManager.BallActive -= ActiveBall;
    }


    void ActiveBall()
    {
        b.SetActive(true);
    }
}
