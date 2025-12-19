using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingToolMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float amplitude = 45f;

    private float initialZRotation;

    private void Start()
    {

        initialZRotation = transform.eulerAngles.z;
    }

    private void Update()
    {

        float angle = Mathf.Sin(Time.time * speed) * amplitude;


        transform.rotation = Quaternion.Euler(initialZRotation + angle, 0, 0);
    }
}
