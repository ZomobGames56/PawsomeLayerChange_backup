using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class TowerLevel_Winner : MonoBehaviour
{
    [SerializeField]
    List<NavMeshAgent> towerLevlAI = new List<NavMeshAgent>();
    [SerializeField]
    Rigidbody playerRef;
    [SerializeField]
    GameObject winnerScreen, loseScreen;

    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    List<Transform> otherPositions = new List<Transform>();
    [SerializeField]
    List<ZLerpMove> insideBox = new List<ZLerpMove>();
    [SerializeField]
    GameObject victoryPos, cloudExit, mainCamera, mainCanvas, game_DL, victoryBoxObj;
    [SerializeField]
    GameObject ballsObj;
    bool crownTouched = false;
    private void Awake()
    {
        victoryBoxObj.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (crownTouched) return;

        if (other.CompareTag("Player") && !crownTouched)
        {
            crownTouched = true;
            ballsObj.SetActive(false);
            playerRef.GetComponent<Rigidbody>().isKinematic = true;
            winnerScreen.SetActive(true);
            HY_Player_Control.canControl = false;
            foreach (NavMeshAgent ai in towerLevlAI)
            {
                ai.GetComponent<NavMeshAgent>().enabled = false;
                ai.GetComponent<UnpredictableClimber>().enabled = false;
                ai.GetComponent<Animator>().enabled = true;
                ai.GetComponent<Animator>().SetTrigger("Defeat");
                ai.GetComponent<Rigidbody>().isKinematic = true;
            }
            if (totalPlayers.Contains(other.gameObject))
            {
                totalPlayers.Remove(other.gameObject);
            }
            StartCoroutine(VictoryBox(playerRef));
        }
        if (other.CompareTag("Enemy") && !crownTouched)
        {
            crownTouched = true;

            ballsObj.SetActive(false);
            playerRef.GetComponent<Rigidbody>().isKinematic = true;
            loseScreen.SetActive(true);
            HY_Player_Control.canControl = false;

            NavMeshAgent _ai = other.gameObject.GetComponent<NavMeshAgent>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.rotation = Quaternion.Euler(0, 0, 0);
            if (totalPlayers.Contains(other.gameObject))
            {
                totalPlayers.Remove(other.gameObject);
            }
            foreach (NavMeshAgent ai in towerLevlAI)
            {
                ai.GetComponent<NavMeshAgent>().enabled = false;
                ai.GetComponent<UnpredictableClimber>().enabled = false;
                if (ai == _ai)
                {
                    ai.GetComponent<Animator>().enabled = true;
                    ai.GetComponent<Animator>().SetTrigger("Victory");
                    ai.GetComponent<NavMeshAgent>().enabled = false;
                    ai.GetComponent<UnpredictableClimber>().enabled = false;
                }
                else
                {
                    ai.GetComponent<Animator>().SetTrigger("Defeat");
                    ai.GetComponent<Animator>().enabled = true;
                    ai.GetComponent<NavMeshAgent>().enabled = false;
                    ai.GetComponent<UnpredictableClimber>().enabled = false;
                }
            }
            playerRef.GetComponent<Animator>().SetTrigger("Defeat");
            StartCoroutine(VictoryBox(rb));

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
        rb.rotation = Quaternion.Euler(0, 0, 0);
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
            if (totalPlayers[i].GetComponent<NavMeshAgent>() != null)
            {
                totalPlayers[i].GetComponent<NavMeshAgent>().enabled = false;

            }
            totalPlayers[i].GetComponent<Animator>().enabled = true;
            totalPlayers[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        playerRef.isKinematic = false;
        playerRef.GetComponent<Animator>().SetTrigger("Victory");
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
