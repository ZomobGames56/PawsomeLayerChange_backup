using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BasicState
{
    None = 0,
    Roam,
    Chase,
    Search,
    Attack,
    Stunned
}

[RequireComponent(typeof(Rigidbody))]
public class m_EnemyHorrorLvl : MonoBehaviour, IDamageable
{
    Rigidbody rb;
    Animator anim;
    BasicState currentBasicState = BasicState.Roam;
    EnemyState currentAnimationState = EnemyState.Idle;
    Vector3 roamTarget;
    public float roamRadius = 6f;

    [Header("Detection")]
    public float visionRange = 10f;
    public float attackRange = 2f;
    public LayerMask targetLayer, groundLayer;
    public Transform target;
    public float moveSpeed = 3f;

    bool isPunching;
    bool isStunned;

    float searchTimer;

    [SerializeField]
    float punchForce = 8f, attackRadius = 0.5f;

    [SerializeField]
    Transform attackPosition;
    [SerializeField]
    GameObject effect;

    const int Max_Result = 5;
    readonly Collider[] hits = new Collider[Max_Result];
    [SerializeField]
    int m_Health = 100;
    [SerializeField]
    GameObject deathEffect;
    [SerializeField]
    GameObject scareCrow;

    [SerializeField]
    Image healthImg;
    Vector3 lastPos;
    float stuckTimer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        healthImg.fillAmount = m_Health / 100;
        PickNewRoamPoint();
    }
    private void Update()
    {
        if (!HY_StartPause.countOver) return;
        if (isStunned) return;

        switch (currentBasicState)
        {
            case BasicState.Attack:
                EnemyAnimationState(EnemyState.HandAttack);
                break;
        }
        Debug.Log($"Current State: {currentBasicState}");
    }
    private void FixedUpdate()
    {
        if (!HY_StartPause.countOver) return;

        CheckStuck();
        if (isStunned) return;
        switch (currentBasicState)
        {
            case BasicState.Roam:
                UpdateRoam();
                EnemyAnimationState(EnemyState.Run);
                break;

            case BasicState.Search:
                UpdateSearch();
                EnemyAnimationState(EnemyState.Idle);
                break;

            case BasicState.Chase:
                UpdateChase();
                EnemyAnimationState(EnemyState.Run);
                break;
        }
    }
    void CheckStuck()
    {
        float distance = Vector3.Distance(transform.position, lastPos);

        if (distance < 0.05f)
        {
            stuckTimer += Time.fixedDeltaTime;

            if (stuckTimer > 1f)
            {
                // 🔥 Force new direction
                PickNewRoamPoint();
                stuckTimer = 0;
            }
        }
        else
        {
            stuckTimer = 0;
        }

        lastPos = transform.position;
    }
    void EnemyAnimationState(EnemyState state, float transtion = 0.25f)
    {
        if (currentAnimationState == state) return;

        currentAnimationState = state;
        anim.CrossFade(state.ToString(), transtion);
    }
    void UpdateRoam()
    {
        MoveTo(roamTarget);

        if (Vector3.Distance(transform.position, roamTarget) < 1f)// add a Timer.
            PickNewRoamPoint();

        if (FindTargert())
        {
            ChangeState(BasicState.Chase);
            Debug.Log("Start Chase");
        }
    }
    void PickNewRoamPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir.y = 0;

        Vector3 candidate = transform.position + randomDir;

        // Raycast only against ground
        if (Physics.Raycast(candidate + Vector3.up * 5f,
                            Vector3.down,
                            out RaycastHit hit,
                            10f,
                            groundLayer))
        {
            roamTarget = hit.point;
        }
        else
        {
            roamTarget = transform.position; // fallback
        }
    }

    #region MOVE
    void MoveTo(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;

        // 🔥 Obstacle detection
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(rayOrigin, transform.forward, 1.2f))
        {
            // Try left & right
            Vector3 left = Quaternion.Euler(0, -45, 0) * transform.forward;
            Vector3 right = Quaternion.Euler(0, 45, 0) * transform.forward;

            bool leftBlocked = Physics.Raycast(rayOrigin, left, 1f);
            bool rightBlocked = Physics.Raycast(rayOrigin, right, 1f);

            if (!leftBlocked)
                dir = left;
            else if (!rightBlocked)
                dir = right;
            else
                dir = -transform.forward; // 🔥 fallback (go back)
        }

        rb.linearVelocity = new Vector3(dir.x * moveSpeed, rb.linearVelocity.y, dir.z * moveSpeed);

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
    }
    #endregion

    #region DETECT
    bool FindTargert()
    {
        Vector3 pos = transform.position;

        int count = Physics.OverlapSphereNonAlloc(pos, visionRange, hits, targetLayer);
        float closest = Mathf.Infinity;
        Transform best = null;
        for (int i = 0; i < count; i++)
        {
            Collider h = hits[i];
            if (h.transform.root == transform.root) continue;
            if (!h.TryGetComponent<IDamageable>(out IDamageable dmg)) continue;

            float distance = (h.transform.position - transform.position).sqrMagnitude;
            if (distance < closest)
            {
                closest = distance;
                best = h.transform;
            }

        }
        target = best;
        return target != null;

    }
    #endregion

    #region CHASE
    void UpdateChase()
    {
        if (!target)
        {
            ChangeState(BasicState.Search);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float sqrDis = dir.sqrMagnitude;

        float attackSqr = attackRange * attackRange;
        float visionSqr = visionRange * visionRange;

        // ATTACK
        if (sqrDis <= attackSqr)
        {
            float dot = Vector3.Dot(transform.forward, dir.normalized);

            if (dot < 0.9f)
            {
                RotateTo(target.position);
                return;
            }

            if (!isPunching)
            {
                rb.linearVelocity = Vector3.zero;
                ChangeState(BasicState.Attack);
                StartCoroutine(PunchRoutine());
            }
            return;
        }

        // FIND PLAYER 
        if (sqrDis <= visionSqr)
        {
            MoveTo(target.position);
            return;
        }
        // LOST PLAYER
        //if (sqrDis > visionSqr)
        //{
        //    ChangeState(BasicState.Search);
        //    Debug.Log("Lost");
        //   // searchTimer = 1f;
        //    return;
        //}

        // CHASE MOVE
        MoveTo(target.position);
    }


    #endregion

    #region SEARCH
    void UpdateSearch()
    {
        searchTimer -= Time.deltaTime;

        if (FindTargert())
        {
            ChangeState(BasicState.Chase);
            return;
        }

        if (searchTimer <= 0)
            ChangeState(BasicState.Roam);
    }
    #endregion
    void RotateTo(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
    }
    #region ATTACK
    IEnumerator PunchRoutine()
    {
        isPunching = true;
        rb.linearVelocity = Vector3.zero;
        Debug.Log("Punch Rotuine");


        yield return new WaitForSeconds(0.5f);

        Collider[] hits = Physics.OverlapSphere(attackPosition.position, attackRadius);

        foreach (var hit in hits)
        {
            if (hit.transform == transform.root) continue;

            Debug.Log(hit.gameObject.name);
            Vector3 effectPos = hit.ClosestPoint(attackPosition.position);

            Instantiate(effect, effectPos, Quaternion.identity);

            if (hit.TryGetComponent<IDamageable>(out var dmg))
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                dir.y += 0.3f;
                dmg.TakeDamage(dir, punchForce, transform);
            }
        }

        yield return new WaitForSeconds(0.6f);

        isPunching = false;
        ChangeState(BasicState.Roam);
    }
    #endregion

    #region STATE
    void ChangeState(BasicState newState)
    {
        if (isStunned) return;
        if (currentBasicState == newState) return;

        currentBasicState = newState;

        switch (newState)
        {
            case BasicState.Roam:
                PickNewRoamPoint();
                break;

            case BasicState.Search:
                rb.linearVelocity = Vector3.zero;
                searchTimer = 2;
                break;

            case BasicState.Chase:
                break;

            case BasicState.Attack:
                rb.linearVelocity = Vector3.zero;
                break;

            case BasicState.Stunned:
                rb.linearVelocity = Vector3.zero;
                break;
        }
    }
    #endregion

    #region DAMAGE
    public void TakeDamage(Vector3 dir, float force, Transform attacker = null)
    {
        // if (isPunching) return; // cannot stun while punching
        if (isStunned) return;
        target = attacker;
        m_Health -= 10;
        healthImg.fillAmount = (float)m_Health / 100;
        Debug.Log(m_Health);
        StartCoroutine(StunRoutine(dir, force));
    }

    IEnumerator StunRoutine(Vector3 dir, float force)
    {
        isStunned = true;
        ChangeState(BasicState.Stunned);

        rb.AddForce(dir * force, ForceMode.Impulse);

        if (m_Health <= 0)
        {
            Die();
            yield break;
        }
        EnemyAnimationState(EnemyState.Attacked_2);

        yield return new WaitForSeconds(1.5f);
        EnemyAnimationState(EnemyState.Idle, 0.5f);
        yield return new WaitForSeconds(0.5f);
        isStunned = false;
        ChangeState(BasicState.Roam);
    }
    #endregion
    void Die()
    {
        EnemyAnimationState(EnemyState.Attacked);

        StartCoroutine(AfterDie());
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 effectPos = transform.position;
        effectPos.y += 1;
        Instantiate(deathEffect, effectPos, Quaternion.Euler(-90, 0, 0));
        yield return new WaitForSeconds(2f);
        Instantiate(scareCrow, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        Horror_LvL_UIManager.AddCountDead();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        pos.y += 1f;
        Gizmos.DrawWireSphere(pos, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);

    }
}
