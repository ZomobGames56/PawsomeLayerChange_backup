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
                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                break;
            case "Enemy":
                //effect show
                Instantiate(effect, other.transform.position, Quaternion.Euler(90, 0, 0));
                other.gameObject.SetActive(false);
                enemyDeathCount++;
                count = enemyDeathCount;
                eliminationTxt.text = count + "/6".ToString();
                break;
        }

    }

}
