using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HY_MainIMGChange : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<Sprite> levelImgs = new List<Sprite>();
    float time;
    public int index;
    int randomTimeToWait, sceneIndexToLoad;
    [SerializeField]
    Image mainImg;
    bool canRun;
    private void Start()
    {
        canRun = true;
        randomTimeToWait = Random.Range(5, 11);
        sceneIndexToLoad = Random.Range(1, 4);
        mainImg = GetComponent<Image>();
        StartCoroutine(WaitToLoadScene());
    }
    // Update is called once per frame
    void Update()
    {
        if (canRun)
        {
            time += Time.deltaTime*3;
            index = (int)time;
            if (index >= levelImgs.Count)
            {
                time = 0;
                index = 0;
            }
            mainImg.sprite = levelImgs[index];
        }
        // Debug.Log(index);

    }
    IEnumerator WaitToLoadScene()
    {
        yield return new WaitForSeconds(randomTimeToWait);
        canRun = false;
        mainImg.sprite = levelImgs[sceneIndexToLoad - 1];
        SceneManager.LoadScene(sceneIndexToLoad);
        
    }
}
