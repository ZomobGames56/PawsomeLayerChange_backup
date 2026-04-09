using System;
using System.Collections;
using UnityEngine;

public class HY_PlayerRagdollActive : MonoBehaviour, IHitAble
{
    public static HY_PlayerRagdollActive instance;
    Rigidbody[] childRbs;
    Animator animator;
    [SerializeField]
    SkinnedMeshRenderer playerRenderedBody;
    [SerializeField]
    MeshRenderer[] childMeshes;
    [SerializeField]
    Transform hip;

    public GameObject Parent;
    [SerializeField] GameObject effect;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Rigidbody _hip, parentRb;
    [SerializeField]
    GameObject waterSplash;
    Coroutine coroutine;
    //public HY_NavMeshEnemy _refNavMesh;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        childRbs = GetComponentsInChildren<Rigidbody>();
        EnableKinamatic();
        _hip = GetComponent<Rigidbody>();
        animator = GetComponentInParent<Animator>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ResetRagoll(0));
        }
    }
    void EnableKinamatic()
    {
        foreach (var child in childRbs)
        {
            child.isKinematic = true;
            child.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    void DisableKinamatic()
    {
        foreach (var child in childRbs)
        {
            child.isKinematic = false;
            child.constraints = RigidbodyConstraints.None;
        }
    }
    IEnumerator ResetRagoll(float wait)
    {
        yield return new WaitForSeconds(wait);
        parentRb.constraints = RigidbodyConstraints.FreezeRotationX |
                               RigidbodyConstraints.FreezeRotationY |
                               RigidbodyConstraints.FreezeRotationZ;
        Parent.transform.position = transform.position;
        print("Repose");
        animator.enabled = true;
        HY_Player_Control.canControl = true;
        Debug.Log("Just called");
        foreach (var child in childRbs)
        {
            child.isKinematic = true;
            child.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    public void RagdollActivate()
    {
        parentRb.constraints = RigidbodyConstraints.FreezeAll;
        animator.enabled = false;
        DisableKinamatic();
     //  coroutine= StartCoroutine(ResetRagoll(3f));
        HY_Player_Control.canControl = false;
        Debug.Log("Just called");
    }
    // [System.Obsolete]
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                HY_Player_Control.canControl = false;
                animator.enabled = false;
                DisableKinamatic();
                parentRb.constraints = RigidbodyConstraints.FreezeAll;
                // StartCoroutine(ResetRagoll(5f));
                Debug.Log("Ragdoll " + gameObject.name);

                break;
            //case "Water":
            //    parentRb.constraints = RigidbodyConstraints.FreezeRotationX |
            //                           RigidbodyConstraints.FreezeRotationY |
            //                           RigidbodyConstraints.FreezeRotationZ;
            //    Parent.transform.position = transform.position;
            //    animator.enabled = true;
            //    HY_Player_Control.canControl = true;
            //    Debug.Log("Just called");
            //    foreach (var child in childRbs)
            //    {
            //        child.isKinematic = true;
            //        child.constraints = RigidbodyConstraints.FreezeAll;
            //    }
            //    break;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {

            // water splash
            // hip kinematic on 

            _hip.isKinematic = true;
           
            parentRb.isKinematic = true;
            ShowPlayer(false);
            //animator.enabled = true;
            //HY_Player_Control.canControl = true;
            //Coroutine coroutine = ResetRagoll(2.5f);
            //foreach (var child in childRbs)
            //{
            //    child.isKinematic = true;
            //    child.constraints = RigidbodyConstraints.FreezeAll;
            //}

            //x Vector3 collisionpoint = other.ClosestPoint(transform.position);
            Instantiate(effect,transform.position, Quaternion.Euler(90, 0, 0));
            //Parent.transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5f);
            //Parent.transform.localScale = Vector3.zero;
            //StartCoroutine(ResetRagoll(0));
            //_hip.linearVelocity = Vector3.zero;
            //parentRb.position = transform.position;
            StartCoroutine(RagDollWater());
            print("Trigger one");
        }
    }
    void ShowPlayer(bool activeState)
    {
        foreach (MeshRenderer m in childMeshes)
        {
            m.enabled = activeState;
        }
        playerRenderedBody.GetComponent<SkinnedMeshRenderer>().enabled = activeState;
    }
   
    IEnumerator RagDollWater()
    {
        yield return new WaitForSeconds(0.55f);
        animator.enabled = true;
        _hip.position = spawnPoint.position;
        Parent.transform.rotation = spawnPoint.rotation;
        parentRb.position = _hip.position;
        ShowPlayer(true);
        //Parent.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        
        parentRb.isKinematic = false;

        parentRb.constraints = RigidbodyConstraints.FreezeRotationX |
                              RigidbodyConstraints.FreezeRotationY |
                              RigidbodyConstraints.FreezeRotationZ;


        //animator.enabled = true;
        HY_Player_Control.canControl = true;
        Debug.Log("Fixed Called");
        foreach (var child in childRbs)
        {
            child.isKinematic = true;
            child.constraints = RigidbodyConstraints.FreezeAll;
        }

    }
    public void OnObstacleCollide()
    {
        HY_Player_Control.canControl = false;
        animator.enabled = false;
        DisableKinamatic();
        StartCoroutine(ResetRagoll(2.5f));
    }

    public void ApplyKnoackBackForce(Vector3 knockBackDirection, float impactForce, float ImpactMultiplier)
    {
        RagdollActivate();
        _hip.AddForce(knockBackDirection * impactForce * ImpactMultiplier, ForceMode.Impulse);
    }
}


