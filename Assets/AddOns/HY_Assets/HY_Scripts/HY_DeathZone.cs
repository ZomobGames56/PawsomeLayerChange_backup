using TMPro;
using UnityEngine;

public class HY_DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    public static int enemyDeathCount;
    public int count;
    [SerializeField]
    GameObject effect;
    [SerializeField]
    TextMeshProUGUI eliminationTxt;
    void Start()
    {
        enemyDeathCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // if enemy falls collider will check how many enemy alive 
        count = enemyDeathCount;
        eliminationTxt.text=count+"/9".ToString();
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                //efect show
                //set Deactive
                //Eliminate Text Shown
                other.gameObject.SetActive(false);
                enemyDeathCount = -1;
                Instantiate(effect, other.transform.position, Quaternion.EulerRotation(90, 0, 0));
                break;
            case "Enemy":
                //effect show
                Instantiate(effect, other.transform.position, Quaternion.EulerRotation(90, 0, 0));
                other.gameObject.SetActive(false);
                enemyDeathCount++;
                break;
        }

    }
}
