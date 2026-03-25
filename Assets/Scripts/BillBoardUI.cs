using UnityEngine;

public class BillBoardUI : MonoBehaviour
{
    Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!cam) return;

        Vector3 dir = transform.position - cam.transform.position;
        dir.y = 0f;

        transform.rotation = Quaternion.LookRotation(dir);
    }
}

