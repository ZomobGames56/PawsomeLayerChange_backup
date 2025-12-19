using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HY_FPS_Check : MonoBehaviour
{
    // Start is called before the first frame update
    float fps = 0.0f;
    [SerializeField]
    TextMeshProUGUI fpsText;
    void Start()
    {
        Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
        //fps += (Time.deltaTime - fps) * 0.1f;

        //fpsText.text = fps.ToString();

        float current = 0;

        current = Time.frameCount / Time.time;
        fps = (int)current;
        fpsText.text = fps.ToString() + " FPS";
    }
}
