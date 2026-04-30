using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class WheelLevel_Winner : MonoBehaviour
{
    [SerializeField]
    GameObject winScreen, loseScreen;
    [SerializeField]
    GameObject playerRef;
    Rigidbody playerRb;
    [SerializeField]
    WheelRotation_AI[] wheelAIRef;
    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    List<Transform> otherPositions = new List<Transform>();
    [SerializeField]
    List<ZLerpMove> insideBox = new List<ZLerpMove>();
    [SerializeField]
    GameObject victoryPos, cloudExit, mainCamera, mainCanvas, game_DL, victoryBoxObj;
    [SerializeField]
    GameObject rotator;
    bool gameOver = false;
    private void Start()
    {
        playerRb = playerRef.GetComponent<Rigidbody>();
        gameOver = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;
        if (other.CompareTag("Player"))
        {
            //
            gameOver = true;
            HY_Player_Control.canControl = false;
            playerRb.isKinematic = true;
            rotator.GetComponent<HY_RotateObstacles>().canRotate = false;
            foreach (WheelRotation_AI ai in wheelAIRef)
            {
                ai.GetComponent<Rigidbody>().isKinematic = true;
                ai.GetComponent<WheelRotation_AI>().enabled = false;
                ai.GetComponent<Animator>().enabled = true;
                ai.GetComponent<Animator>().SetTrigger("Defeat");
            }
            winScreen.SetActive(true);
            if (totalPlayers.Contains(other.gameObject))
            {
                totalPlayers.Remove(other.gameObject);
            }
            StartCoroutine(VictoryBox(playerRb));
        }
        if (other.CompareTag("Enemy"))
        {

            gameOver = true;

            HY_Player_Control.canControl = false;
            playerRb.isKinematic = true;
            loseScreen.SetActive(true);
            rotator.GetComponent<HY_RotateObstacles>().canRotate = false;
            Rigidbody enmyRb = other.gameObject.GetComponent<Rigidbody>();
            WheelRotation_AI _ai = other.gameObject.GetComponent<WheelRotation_AI>();
            if (totalPlayers.Contains(other.gameObject))
            {
                totalPlayers.Remove(other.gameObject);
            }
            for (int i = totalPlayers.Count - 1; i >= 0; i--)
            {
                if (totalPlayers[i].name == enmyRb.gameObject.name)
                {
                    totalPlayers.RemoveAt(i);
                }
            }

            foreach (WheelRotation_AI ai in wheelAIRef)
            {
                ai.GetComponent<Rigidbody>().isKinematic = true;
                ai.GetComponent<Animator>().enabled = true;
                ai.GetComponent<WheelRotation_AI>().enabled = false;
                if (ai == _ai)
                {
                    ai.GetComponent<Animator>().SetTrigger("Victory");

                }
                else
                {
                    ai.GetComponent<Animator>().SetTrigger("Defeat");

                }

            }
            //other.gameObject.GetComponent<Animator>().SetTrigger("Victory");
            playerRef.GetComponent<Animator>().SetTrigger("Defeat");
            StartCoroutine(VictoryBox(enmyRb));
        }
    }

    IEnumerator VictoryBox(Rigidbody rb)
    {
        yield return new WaitForSeconds(3f);
        //players repose
        //main camera false
        //canvas false
        // direaction light false
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
