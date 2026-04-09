using UnityEngine;
using UnityEngine.UI;
public class ImageStrechter : MonoBehaviour
{
    private float initialRatio , finalRatio;
    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialRatio = rectTransform.rect.width / rectTransform.rect.height;

    }
    private void Update()
    {

        finalRatio = Screen.width / Screen.height;
        if(initialRatio > finalRatio)
        {
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width/initialRatio);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(Screen.height * initialRatio, Screen.height);
        }
    }
}
