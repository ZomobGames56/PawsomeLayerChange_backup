using UnityEngine;
using UnityEngine.SceneManagement;

public class TempGameManager : MonoBehaviour
{

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
