using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class HorrorLevelFallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.SetActive(false);
            Horror_LvL_UIManager.PlayerDeadCheck();
            StartCoroutine(LevelSelection());
        }
        if (other.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
            Horror_LvL_UIManager.AddCountDead();
        }
        if (other.CompareTag("Ground"))
        {
            other.gameObject.SetActive(false);
        }
    }
    IEnumerator LevelSelection()
    {
        yield return new WaitForSeconds(3f);
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
