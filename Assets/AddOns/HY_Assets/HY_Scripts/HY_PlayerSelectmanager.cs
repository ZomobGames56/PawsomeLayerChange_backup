using UnityEngine;

public class HY_PlayerSelectmanager : MonoBehaviour
{
    [SerializeField]
    GameObject[] models;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        DeactiveAll();
        //get saved data from player prefs.....
        models[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            index--;
            if (index < 0)
            {
                index = models.Length-1;
            }
            DeactiveAll();
            models[index].SetActive(true);
            print(index);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            index++;
            if (index > models.Length - 1)
            {
                index = 0;
            }
            DeactiveAll();
            models[index].SetActive(true);
            print(index);
        }

    }
    void DeactiveAll()
    {
        foreach (GameObject model in models)
        {
            model.SetActive(false);
        }
    }
}
