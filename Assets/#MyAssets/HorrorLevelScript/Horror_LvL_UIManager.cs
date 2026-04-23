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
        get { return _instance; }
    }

    // variable
    [SerializeField]
    TextMeshProUGUI eliminaterdTxt;
    [SerializeField]
    int count;
    [SerializeField]
    List<GameObject> totalPlayers = new List<GameObject>();
    [SerializeField]
    List<Transform> otherPositions = new List<Transform>();
    [SerializeField]
    List<ZLerpMove> insideBox = new List<ZLerpMove>();
    [SerializeField]
    GameObject winScreen, loseScreen;
    bool isPlayerDead;
    [SerializeField]
    GameObject victoryPos, cloudExit, mainCamera, mainCanvas, game_DL, victoryBoxObj;
    [SerializeField]
    Rigidbody playerRef;
    [SerializeField]
    PlayerControl playerControl;

    bool died;
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
            ////win situation 
            //// take all the AIs
            //// stop there move | player position to victory pos | remain to other pos.
            //playerControl.CanMove = false;
            //playerRef.GetComponent<Rigidbody>().isKinematic = true;
            //StartCoroutine(VictoryBox(playerRef));

        }

    }
    public static void PlayerDeadCheck()
    {
        _instance.loseScreen.SetActive(true);
        _instance.PlayerLoseCheck();
    }
    void PlayerLoseCheck()
    {
        if (died) return;

        died = true;

        playerRef.isKinematic = false;
        playerControl.CanMove = false;
        playerRef.linearVelocity = Vector3.zero;
        GameObject playerObj = playerRef.gameObject;
        if (!totalPlayers.Contains(playerObj))
        {
            totalPlayers.Insert(0, playerObj);
        }

        int rnd = Random.Range(0, 10);
        Rigidbody rb = totalPlayers[rnd].GetComponent<Rigidbody>();
        print(rnd);
        if (totalPlayers.Contains(rb.gameObject))
        {
            totalPlayers.Remove(rb.gameObject);
        }
        foreach (GameObject obj in totalPlayers)
        {
            if (obj.GetComponent<m_EnemyHorrorLvl>() != null)
            {
                obj.GetComponent<m_EnemyHorrorLvl>().enabled = false;
                obj.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        rb.gameObject.SetActive(true);
        rb.gameObject.GetComponent<m_EnemyHorrorLvl>().enabled = false;
        rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(VictoryBox(rb));
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
            totalPlayers[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            //totalPlayers[i].GetComponent<Rigidbody>().

            totalPlayers[i].transform.SetPositionAndRotation(
                otherPositions[i].position,
                otherPositions[i].rotation
            );
            if (totalPlayers[i].GetComponent<m_EnemyHorrorLvl>() != null)
            {
                totalPlayers[i].GetComponent<m_EnemyHorrorLvl>().enabled = false;

            }
            totalPlayers[i].SetActive(true);
            totalPlayers[i].GetComponent<Animator>().Play("Idle");
            totalPlayers[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        playerRef.isKinematic = false;
        playerRef.GetComponent<Animator>().Play("Idle");
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
