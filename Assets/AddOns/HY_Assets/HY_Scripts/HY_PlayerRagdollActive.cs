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
    Transform spawnPoint, firstSp, secondSp, thirdSp, fourthSp,cloneSpawnPoint;
    [SerializeField]
    Rigidbody _hip, parentRb;
    [SerializeField]
    GameObject waterSplash;
    Coroutine coroutine;
    public bool collideWater = false;
    bool once, ragdollActive;

    //public HY_NavMeshEnemy _refNavMesh;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        once = false;
        ragdollActive = false;
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

    public void RagdollActivate()
    {
        if (ragdollActive) return;

        ragdollActive |= true;
        parentRb.constraints = RigidbodyConstraints.FreezeAll;
        animator.enabled = false;
        DisableKinamatic();
        HY_Player_Control.canControl = false;
        coroutine = StartCoroutine(RagDollWater(3f));
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
                //StartCoroutine(ResetRagoll(5f));
                Debug.Log("Ragdoll " + gameObject.name);

                break;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water")&& !once)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            once = true;
            collideWater = true;
            _hip.isKinematic = true;

            parentRb.isKinematic = true;
            ShowPlayer(false);

            Instantiate(effect, transform.position, Quaternion.Euler(90, 0, 0));
            StartCoroutine(RagDollWater(1f));
            print("Trigger one");
        }

        switch (other.tag)
        {
            case "FirstSp":
                spawnPoint = firstSp;
                cloneSpawnPoint = firstSp;
                break;
            case "SecondSp":
                spawnPoint = secondSp;
                cloneSpawnPoint = firstSp;

                break;
            case "ThirdSp":
                spawnPoint = thirdSp;
                cloneSpawnPoint = thirdSp;
                break;
            case "FourthSp":
                spawnPoint = fourthSp;
                cloneSpawnPoint = fourthSp;
                break;


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
    IEnumerator ResetRagoll(float wait)
    {
        yield return new WaitForSeconds(wait);
        animator.enabled = true;
        parentRb.constraints = RigidbodyConstraints.FreezeRotationX |
                               RigidbodyConstraints.FreezeRotationY |
                               RigidbodyConstraints.FreezeRotationZ;

        Parent.transform.position = spawnPoint.position;
      
        HY_Player_Control.canControl = true;
       
        foreach (var child in childRbs)
        {
            child.isKinematic = true;
            child.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    IEnumerator RagDollWater(float wait)
    {
        yield return new WaitForSeconds(wait);
        animator.enabled = true;
        once = false;
        ragdollActive = false;
        parentRb.constraints = RigidbodyConstraints.FreezeRotationX |
                              RigidbodyConstraints.FreezeRotationY |
                              RigidbodyConstraints.FreezeRotationZ;
        if (!collideWater)
        {
            _hip.position = transform.position;
            //parentRb.rotation = spawnPoint.rotation;
            parentRb.position = _hip.position;

        }
        if (collideWater)
        {
            _hip.position = spawnPoint.position;
            //parentRb.rotation = spawnPoint.rotation;
            parentRb.position = _hip.position;
        }

        
        ShowPlayer(true);
        parentRb.isKinematic = false;
        collideWater = false;
      
        foreach (var child in childRbs)
        {
            child.isKinematic = true;
            child.constraints = RigidbodyConstraints.FreezeAll;
        }
        HY_Player_Control.canControl = true;

    }
    public void OnObstacleCollide()
    {
        HY_Player_Control.canControl = false;
        animator.enabled = false;
        DisableKinamatic();
        //StartCoroutine(ResetRagoll(2.5f));
        StartCoroutine(RagDollWater(2.5f));
    }

    public void ApplyKnoackBackForce(Vector3 knockBackDirection, float impactForce, float ImpactMultiplier)
    {
        RagdollActivate();
        _hip.AddForce(knockBackDirection * impactForce * ImpactMultiplier, ForceMode.Impulse);
    }
}


