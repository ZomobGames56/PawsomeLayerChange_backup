using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class HY_BricksBehaviour : MonoBehaviour
{// Start is called before the first frame update
  
    [SerializeField]
    Material red, white;
    MeshRenderer mr;
    float time;
    bool go;
    [SerializeField]
    float timeMultiplier=2.0f;
    [SerializeField]
    AudioClip brickSound;
    bool playOnce;
    [SerializeField]
    float waitTime;
    void Start()
    {
        playOnce = true;
        mr = GetComponent<MeshRenderer>();
       // mr.material = white;
    }

    // Update is called once per frame
    void Update()
    {
        if (go)
        {
            time += Time.deltaTime * timeMultiplier;
            if (transform.localScale.x >= 0f)
            {
                transform.localScale -= new Vector3(time, time, time);
                time = 0f;
            }
            if (transform.localScale.x < 0f)
            {
                transform.localScale = Vector3.zero;
            }
        }

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
    //    {
    //        transform.localPosition=new Vector3(transform.localPosition.x,-0.01f,transform.localPosition.z);
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
    //    {
    //        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    //    }
    //}
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            mr.material = red;
            //transform.localPosition = new Vector3(transform.localPosition.x, -0.01f, transform.localPosition.z);
            if (HY_StartPause.countOver == true)
            {
                StartCoroutine(WaitMan());
            }
            //transform.localScale = new Vector3(1f,.25f, 1f);
          
        }
    }
    
    
    IEnumerator WaitMan()
    {
        yield return new WaitForSeconds(waitTime);
        if (playOnce)
        {
            HY_AudioManager.instance.PlayAudioEffectOnce(brickSound);
            Debug.Log("Sound Played");
            playOnce=false;
        }
        go = true;
    }


}
