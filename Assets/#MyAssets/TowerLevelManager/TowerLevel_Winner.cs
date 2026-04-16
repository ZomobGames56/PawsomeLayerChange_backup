using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TowerLevel_Winner : MonoBehaviour
{
    [SerializeField]
   List<NavMeshAgent> towerLevlAI = new List<NavMeshAgent>();
    [SerializeField]
    Rigidbody playerRef;
    [SerializeField]
    GameObject winnerScreen, loseScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRef.GetComponent<Rigidbody>().isKinematic = true;
            winnerScreen.SetActive(true);
            foreach (NavMeshAgent ai in towerLevlAI)
            {
                ai.GetComponent<NavMeshAgent>().enabled = false;
                ai.GetComponent<UnpredictableClimber>().enabled = false;
            }
            StartCoroutine(LevelSelectionScene());
        }
        if (other.CompareTag("Enemy"))
        {
            playerRef.GetComponent<Rigidbody>().isKinematic = true;
            loseScreen.SetActive(true);
            foreach (NavMeshAgent ai in towerLevlAI)
            {
                ai.GetComponent<NavMeshAgent>().enabled = false;
                ai.GetComponent<UnpredictableClimber>().enabled = false;
            }
            StartCoroutine(LevelSelectionScene());
        }
    }
    IEnumerator LevelSelectionScene()
    {

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(6);

    }
}
