using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WoodenLevelWinner : MonoBehaviour
{
    int count = 0;
    [SerializeField]
    List<GameObject> enemyList = new List<GameObject>();
    [SerializeField]
    GameObject winPanel, losePanel;
    [SerializeField]
    Rigidbody playerRb;
    [SerializeField]
    Rigidbody[] enemyObject;
    private void Start()
    {
        //playerRb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Player":
                //player lost.
                collision.gameObject.SetActive(false);
                losePanel.SetActive(true);
                foreach (Rigidbody r in enemyObject)
                {
                    r.GetComponent<Rigidbody>().isKinematic = true;
                }
                StartCoroutine(LevelSelectionScene());
                break;
            case "Enemy":
                collision.gameObject.SetActive(false);
                count++;
                if (count == enemyList.Count)
                {
                    winPanel.SetActive(true);
                    playerRb.GetComponent<Rigidbody>().isKinematic = true;

                    StartCoroutine(LevelSelectionScene());
                }
                break;
        }
    }


    IEnumerator LevelSelectionScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(6);
    }
}
