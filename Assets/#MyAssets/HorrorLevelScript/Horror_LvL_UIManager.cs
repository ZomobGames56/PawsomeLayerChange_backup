using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class Horror_LvL_UIManager : MonoBehaviour
{
    private static Horror_LvL_UIManager _instance;

    public static Horror_LvL_UIManager Instance
    {
        get {  return _instance; }
    }

    // variable
    [SerializeField]
    TextMeshProUGUI eliminaterdTxt;
    [SerializeField]
    int count;
    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    GameObject winScreen, loseScreen;
    bool isPlayerDead;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }
    private void Start()
    {
        count = 0;
        //eliminaterdTxt.text = count.ToString()+"/"+ totalPlayers.Count.ToString();
        eliminaterdTxt.text = $"{count.ToString()}/{totalPlayers.Count.ToString()}";
    }
    void UpdateText()
    {
        eliminaterdTxt.text = $"{count.ToString()}/{totalPlayers.Count.ToString()}";
    }
    public static void AddCountDead()
    {
        _instance.count++;
        _instance.UpdateText();
        _instance.CheckAllDead();
    }
    void CheckAllDead()
    {
        if (count == totalPlayers.Count)
        {
            winScreen.SetActive(true);
            StartCoroutine(LevelSelection());
        }
    }
    public static void PlayerDeadCheck()
    {
        _instance.loseScreen.SetActive(true);
       
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
