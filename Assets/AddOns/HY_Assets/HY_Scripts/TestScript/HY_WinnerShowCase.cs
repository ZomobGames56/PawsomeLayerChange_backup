using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HY_WinnerShowCase : MonoBehaviour
{
    public static HY_WinnerShowCase instance;

    [SerializeField]
    List<GameObject> stoneModels = new List<GameObject>();
    [SerializeField]
    Material defaultMaterial, greenMaterial,redMaterial;
    [SerializeField]
    GameObject playerModel;
    [SerializeField]
    RuntimeAnimatorController controller;
    [SerializeField]
    GameObject tvScreen;
    [SerializeField]
    Material qualified, disqualified;
    [SerializeField]
    float timeForChangeScene;
    public bool isPlayerWon;
    [SerializeField]
    AudioClip tileSound,winSound;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
       
    }
    void Start()
    {
        HY_AudioManager.instance.PlayAudioEffectOnce(tileSound);
       playerModel.GetComponent<Animator>().runtimeAnimatorController = controller;
       playerModel.GetComponent<Animator>().enabled = true;
        for (int i = 0; i < stoneModels.Count; i++)
        {
            stoneModels[i].GetComponent<MeshRenderer>().material = defaultMaterial;
        }
        StartCoroutine(ChangeColor());

    }
    private void Update()
    {
        playerModel.GetComponent<Animator>().enabled = true;
    }
    IEnumerator ChangeColor()
    {
        float speed = .5f;
        ResetAll();
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < 5; i++)
        {
            foreach (GameObject go in stoneModels)
            {
                ResetAll();
                go.GetComponent<MeshRenderer>().material = greenMaterial;
                yield return new WaitForSeconds(speed);
            }
            speed = speed / 2;
            yield return new WaitForSeconds(.1f);
            ResetAll();
        }
        if (isPlayerWon)
        {
            playerModel.GetComponent<Animator>().SetBool("isWin", true);
            tvScreen.GetComponentInParent<Animator>().enabled = false;
            tvScreen.GetComponent<MeshRenderer>().material = qualified;
            stoneModels[1].GetComponent<MeshRenderer>().material = greenMaterial;
            stoneModels[1].GetComponent<Rigidbody>().isKinematic = true;
            
            for (int j = 0; j < stoneModels.Count; j++)
            {
                if (j == 1)
                {
                    yield return null;
                }
                else
                {
                    stoneModels[j].GetComponent<MeshRenderer>().material = redMaterial;
                    stoneModels[j].GetComponent<Rigidbody>().isKinematic = false;

                }
            }
            StartCoroutine(ChangeSceneTOLevel());


        }
        else if (!isPlayerWon)
        {
            print("Called");
            playerModel.GetComponent<Animator>().SetBool("isDead", true);
            tvScreen.GetComponentInParent<Animator>().enabled = false;
            tvScreen.GetComponent<MeshRenderer>().material = disqualified;
            int rnd = Random.Range(2, stoneModels.Count);
            stoneModels[rnd].GetComponent<MeshRenderer>().material = greenMaterial;
            stoneModels[rnd].GetComponent<Rigidbody>().isKinematic = true;
           
            for (int j = 0; j < stoneModels.Count; j++)
            {
                if (j == rnd)
                {
                    yield return null;
                }
                else
                {
                    stoneModels[j].GetComponent<MeshRenderer>().material = redMaterial;
                    stoneModels[j].GetComponent<Rigidbody>().isKinematic = false;
                }
                HY_AudioManager.instance.PlayAudioEffectOnce(winSound);
            }

            StartCoroutine(ChangeSceneTOLevel());

        }

    }

    IEnumerator ChangeSceneTOLevel()
    {
        yield return new WaitForSeconds(timeForChangeScene);
        SceneManager.LoadScene(6);
    }
    void ResetAll()
    {
        for (int i = 0; i < stoneModels.Count; i++)
        {
            stoneModels[i].GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }
}
