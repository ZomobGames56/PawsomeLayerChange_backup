using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HorrorLevelFallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.SetActive(false);
            Horror_LvL_UIManager.PlayerDeadCheck();
            StartCoroutine(LevelSelection("_Level_Selection"));
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
    IEnumerator LevelSelection(string t)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(t);

    }
}
