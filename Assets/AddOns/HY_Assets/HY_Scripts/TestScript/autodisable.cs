using System.Collections;
using UnityEngine;

public class autodisable : MonoBehaviour
{
    public float time;

    private void OnEnable()
    {
        StartCoroutine(Disable(time));
    }
    // Start is called before the first frame update
    public void buttondisable()
    {
        StartCoroutine(Disable(time));
    }
    public IEnumerator Disable(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }
}
