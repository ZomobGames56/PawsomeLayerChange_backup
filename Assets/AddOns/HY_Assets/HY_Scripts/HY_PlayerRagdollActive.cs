using System.Collections;
using UnityEngine;

public class HY_PlayerRagdollActive : MonoBehaviour
{
    public static HY_PlayerRagdollActive instance;
    Rigidbody[] childRbs;
    Animator animator;

    [SerializeField]
    Transform hip;

    public GameObject Parent;
    [SerializeField] GameObject effect;
    Transform spawnPoint;
    [SerializeField]
    Rigidbody parentPlayerRb, hipRB;
    //public HY_NavMeshEnemy _refNavMesh;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        childRbs = GetComponentsInChildren<Rigidbody>();
        EnableKinamatic();
        animator = GetComponentInParent<Animator>();
        parentPlayerRb = parentPlayerRb.GetComponentInParent<Rigidbody>();
        hipRB = GetComponent<Rigidbody>();
        parentPlayerRb.constraints = RigidbodyConstraints.FreezeRotation;
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
        parentPlayerRb.constraints = RigidbodyConstraints.None |
                                          RigidbodyConstraints.FreezeRotation;
        animator.enabled = true;
        parentPlayerRb.position = hipRB.position;
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
        animator.enabled = false;
        DisableKinamatic();
        StartCoroutine(ResetRagoll(3f));
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
                // StartCoroutine(ResetRagoll(5f));
                Debug.Log("Collide Obstacle " + gameObject.name);
                break;

            case "Water":
                Debug.Log("Water");
                break;

        }


    }

    public void OnObstacleCollide()
    {
        parentPlayerRb.constraints = RigidbodyConstraints.FreezePositionX |
                                     RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotation;

        HY_Player_Control.canControl = false;
        animator.enabled = false;
        DisableKinamatic();
        StartCoroutine(ResetRagoll(3f));
    }


}


