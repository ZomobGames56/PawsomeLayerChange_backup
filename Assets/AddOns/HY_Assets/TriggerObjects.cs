using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObjects : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> objects = new List<GameObject>();

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log(other.name);
        }
    }
}
