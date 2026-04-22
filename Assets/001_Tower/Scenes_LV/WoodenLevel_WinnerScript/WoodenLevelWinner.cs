using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
    [SerializeField]
    GameObject victoryPos, cloudExit;
    [SerializeField]
    GameObject mainCamera, mainCanvas, game_DL, victoryBoxObj;
    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    List<Transform> otherPositions = new List<Transform>();
    [SerializeField]
    List<ZLerpMove> insideBox = new List<ZLerpMove>();
    bool collide;
    private void Start()
    {
        //playerRb = GetComponent<Rigidbody>();
        victoryBoxObj.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Player":
                if (collide) return;

                collide = true;
                //player lost.
                collision.gameObject.SetActive(false);
                losePanel.SetActive(true);
                foreach (Rigidbody r in enemyObject)
                {
                    r.GetComponent<Rigidbody>().isKinematic = true;
                }
                playerRb.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                HY_Player_Control.canControl = false;
                int rnd = Random.Range(1, totalPlayers.Count);
                print(rnd);
                Rigidbody rb = totalPlayers[rnd].GetComponent<Rigidbody>();
                if (totalPlayers.Contains(totalPlayers[rnd]))
                {
                    totalPlayers[rnd].GetComponent<WoodenLV_AI_WithAnimation>().enabled = false;
                    totalPlayers[rnd].GetComponent<Animator>().SetTrigger("Victory");
                    totalPlayers[rnd].SetActive(true);
                    totalPlayers[rnd].transform.position = victoryBoxObj.transform.position;
                    totalPlayers.Remove(totalPlayers[rnd]);
                }
                StartCoroutine(VictoryBox(rb));
                break;
            case "Enemy":
                collision.gameObject.SetActive(false);
                count++;
                if (count == enemyList.Count)
                {
                    winPanel.SetActive(true);
                    playerRb.GetComponent<Rigidbody>().isKinematic = true;
                    playerRb.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    HY_Player_Control.canControl = false;
                    if (totalPlayers.Contains(playerRb.gameObject))
                    {
                        totalPlayers.Remove(playerRb.gameObject);
                    }
                    StartCoroutine(VictoryBox(playerRb));

                }
                break;
        }
    }


    IEnumerator VictoryBox(Rigidbody rb)
    {
        yield return new WaitForSeconds(3f);
       
        rb.position = victoryPos.transform.position;
        rb.rotation = victoryPos.transform.rotation;
        mainCamera.SetActive(false);
        mainCanvas.SetActive(false);
        game_DL.SetActive(false);
        victoryBoxObj.SetActive(true);

        for (int i = 0; i < totalPlayers.Count && i < otherPositions.Count; i++)
        {
            totalPlayers[i].transform.SetPositionAndRotation(
                otherPositions[i].position,
                otherPositions[i].rotation
            );
            totalPlayers[i].SetActive(true);
            totalPlayers[i].GetComponent<Animator>().SetTrigger("Defeat");
            totalPlayers[i].GetComponent<Rigidbody>().isKinematic = false;
            if (totalPlayers[i].GetComponent<WoodenLV_AI_WithAnimation>() != null)
            {
                totalPlayers[i].GetComponent<WoodenLV_AI_WithAnimation>().enabled = false;
            }
        }
        playerRb.isKinematic = false;
        playerRb.GetComponent<Animator>().SetTrigger("Victory");
        yield return new WaitForSeconds(2f);
        foreach (ZLerpMove z in insideBox)
        {
            z.pushOn = true;
        }
        yield return new WaitForSeconds(2f);
        cloudExit.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        var handle = Addressables.LoadSceneAsync("LevelSelection", LoadSceneMode.Single);
        while (!handle.IsDone)
        {
            float percent = handle.PercentComplete;

            yield return null;
        }

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Scene load failed");
        }
    }
}
