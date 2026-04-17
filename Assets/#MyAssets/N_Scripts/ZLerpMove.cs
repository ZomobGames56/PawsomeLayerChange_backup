using UnityEngine;

public class ZLerpMove : MonoBehaviour
{
    public bool pushOn = false;
    private void Start()
    {
       
    }

    void Update()
    {
        if (!pushOn) return;

        Vector3 currentPos = transform.localPosition;
        Vector3 targetPos = new Vector3(currentPos.x, currentPos.y, -175f);

        transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10f);
    }
}