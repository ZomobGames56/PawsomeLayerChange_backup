using UnityEngine;
using UnityEngine.UI;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    Image fillImg;
    float time = 0;
    [SerializeField]
    float pickUpTime;

    bool canCallPickUp;
    [SerializeField]
    Transform headTarget;
    [SerializeField]
    float currentFillAmount;
    void Start()
    {
        fillImg.fillAmount = 0;
        canCallPickUp = true;
    }

    // Update is called once per frame


    void AttackedToHead()
    {
        // Vector3 currentPos = transform.position;
        transform.position = headTarget.position;
        transform.SetParent(headTarget);
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            time += Time.deltaTime;
            fillImg.fillAmount = time / pickUpTime;
            // currentFillAmount = fillImg.fillAmount;
            Debug.Log("Filling Image");

            if (time >= pickUpTime)
            {
                canCallPickUp = false;
                Debug.Log("ImageFilled");
                time = 0;
                fillImg.fillAmount = 0;
                AttackedToHead();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (canCallPickUp)
            {
                fillImg.fillAmount = 0;
                time = 0;
            }
        }
    }

   
}
