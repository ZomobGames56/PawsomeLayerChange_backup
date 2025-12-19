using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    public RectTransform graphContainer;
    public Sprite circleSprite;
    private List<GameObject> pointsList = new List<GameObject>();

    private void Start()
    {
        // Example: Create a graph with random values between 0 and 60
        for (int i = 0; i < 10; i++)
        {
            AddGraphPoint(Random.Range(0, 60));
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        pointsList.Add(gameObject);
        return gameObject;
    }

    public void AddGraphPoint(float value)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float xPosition = pointsList.Count * (graphWidth / 60); // Assume 60 points max
        float yPosition = (value / 60) * graphHeight;
        CreateCircle(new Vector2(xPosition, yPosition));
        if (pointsList.Count > 1)
        {
            CreateLine(pointsList[pointsList.Count - 2].GetComponent<RectTransform>().anchoredPosition,
                       pointsList[pointsList.Count - 1].GetComponent<RectTransform>().anchoredPosition);
        }
    }

    private void CreateLine(Vector2 start, Vector2 end)
    {
        GameObject lineObject = new GameObject("line", typeof(Image));
        lineObject.transform.SetParent(graphContainer, false);
        lineObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = lineObject.GetComponent<RectTransform>();
        Vector2 dir = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f); // 3f is the thickness of the line
        rectTransform.anchoredPosition = start + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}
