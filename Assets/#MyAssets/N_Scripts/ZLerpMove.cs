using UnityEngine;

public class ZLerpMove : MonoBehaviour
{
    //public bool pushOn = false;
    //void Update()
    //{
    //    if (!pushOn) return;

    //    Vector3 currentPos = transform.localPosition;
    //    Vector3 targetPos = new Vector3(currentPos.x, currentPos.y, -175f);

    //    transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10f);
    //}

    public bool pushOn = false;

    float delay;
    float timer;

    void OnEnable()
    {
        // Random delay between 0 to 0.75 seconds
        delay = Random.Range(0f, 0.75f);
        timer = 0f;
    }

    void Update()
    {
        if (!pushOn) return;

        timer += Time.deltaTime;

        // Wait until delay is passed
        if (timer < delay) return;

        Vector3 currentPos = transform.localPosition;
        Vector3 targetPos = new Vector3(currentPos.x, currentPos.y, -175f);

        transform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10f);
    }
}