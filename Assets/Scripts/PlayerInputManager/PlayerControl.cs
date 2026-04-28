using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerState
{
    None = 0,
    Idle,
    Run,
    Jump,
    Hang,
    Dash,
    Swim,
    Throw,
    HandAttack,
    Attacked,
    Attacked_2

}
[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour, IDamageable
{
    #region Variables

    [SerializeField]
    Joystick fixedJoystick;
    Rigidbody rb;
    [Header("Player Move Speed")]
    [SerializeField]
    float moveSpeed = 8;
    [Header("Player Jump Force")]
    [SerializeField]
    float jumpForce = 8;
    //Input Value
    float h, v;
    Vector3 move;
    Animator animator;

    PlayerState currentState = PlayerState.Idle;

    [Header("Attack position ")]
    [SerializeField]
    Transform attackPosition;

    [Header("Attack Radius ")]
    [SerializeField]
    float attackRadius = 2;

    [Header("Attack Cool Down Time")]
    [SerializeField]
    float handAttackCoolDownTime = 2f;

    [Header("Attack Force")]
    [SerializeField]
    float attackForce = 12f;

    int reactionLayer, actionLayer;
    bool isStateLocked = false;

    [Header("Camera Transform")]
    [SerializeField]
    Transform cam;
    [SerializeField]
    GameObject effect;
    bool canAttack = true;

    float holdTimer = 0f;
    bool isHolding = false;
    bool chargedFired = false;

    float chargeTime = 3f;
    [SerializeField]
    Button attackBtn;

    bool isInputStateLock = false;
    public bool isGrounded = true;
    float inAirTimer;
    bool canMove = true;
    [SerializeField]
    int count = 0;
    bool isStunned = false;
    bool isPunching = false;
    [SerializeField] Image img;
    [SerializeField]
    int playerHelath = 100;
    [SerializeField]
    Image healthbarImg;
    [SerializeField]
    GameObject deathEffect;
    [SerializeField]
    GameObject scareCrow;
    [SerializeField]
    float inputDeadZone = 0.9f;
    RaycastHit hit;
    [SerializeField]
    LayerMask layer;
    [SerializeField]
    Transform head;
    int mask;
    bool isDead = false;
    bool isJumping = false;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    #endregion
    private void Awake()
    {
        playerHelath = 100;
        healthbarImg.fillAmount = playerHelath / 100;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        reactionLayer = animator.GetLayerIndex("Reaction Layer");
        actionLayer = animator.GetLayerIndex("Action Layer");


    }
    private void Update()
    {
        h = fixedJoystick.Horizontal;
        v = fixedJoystick.Vertical;

        if (move.magnitude != 0 && canMove)
        {
            Rotate();
        }

        // PunchAttackHandle();
        UpdateLocoMotion();
        Jump();

    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }
    #region Player Movement Handle
    void PlayerMovement()
    {
        if (!canMove) return;

        Vector3 camForwad = cam.forward;
        Vector3 camRight = cam.right;

        camForwad.y = 0;
        camRight.y = 0;
        camForwad.Normalize(); camRight.Normalize();
        move = camRight * h + camForwad * v;
        // move = new Vector3( h, 0, v);
        move = Vector3.ClampMagnitude(move, 1f);
        move.y = 0;
        Vector3 velocity = rb.linearVelocity;
        if (move.magnitude > inputDeadZone)
        {
            Vector3 moveDir = move;
            mask = ~layer;
            if (Physics.Raycast(head.position, moveDir, out hit, 0.6f, mask, QueryTriggerInteraction.Ignore))
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, hit.normal);
            }
            velocity.x = moveDir.x * moveSpeed;
            velocity.z = moveDir.z * moveSpeed;

        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;

        }

        rb.linearVelocity = velocity;

    }
    #endregion
    public void Rotate()
    {
        Quaternion rot = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot,
            720f * Time.deltaTime);
    }

    #region Jump Function
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            if (isPunching) return;
            if (isStunned) return;

            isStateLocked = true;
            isJumping = true;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);//New line added;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //if (isStunned) return;
            PlayerAnimationStateUpdate(PlayerState.Jump, true, 0.05f);
            StartCoroutine(JumpRoutine());
        }
    }

    public void MobileJump()
    {
        if (!isGrounded) return;
        if (isJumping) return;
        if (isPunching) return;
        if (isStunned) return;

        isStateLocked = true;
        isJumping = true;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);//New line added;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //if (isStunned) return;
        PlayerAnimationStateUpdate(PlayerState.Jump, true, 0.2f);
        StartCoroutine(JumpRoutine());
    }
    #endregion
    IEnumerator JumpRoutine()
    {
        // small delay so jump anim shows
        yield return new WaitForSeconds(0.5f);
        if (isStunned) yield break;
        // while in air
        while (!isGrounded)
        {
            if (isStunned) yield break;
            PlayerAnimationStateUpdate(PlayerState.Hang, true, 0.5f);
            yield return null;
        }

        //reseting everything needed;
        isStateLocked = false;
        isJumping = false;
    }
    void PlayerAnimationStateUpdate(PlayerState state, bool lockState = false, float transactionDuration = 0.2f)
    {
        if (isDead) return;
        if (currentState == state) return;
        if (lockState)
        {
            isStateLocked = true;
        }


        currentState = state;
        animator.CrossFade(state.ToString(), transactionDuration);
        //animator.SetInteger((int)state, 1);
    }
    void UpdateLocoMotion(float transactionTime = 0.2f)
    {
        if (isStateLocked || isStunned) return;

        if (InAirCheck())
        {
            PlayerAnimationStateUpdate(PlayerState.Hang, true, 0.5f);
            //Debug.Log("Hang");
            return;
        }

        if (move.magnitude >= inputDeadZone)
        {
            PlayerAnimationStateUpdate(PlayerState.Run, false, 0.1f);

        }
        else
        {
            PlayerAnimationStateUpdate(PlayerState.Idle, false, 0.1f);

        }
    }

    #region Punch Attack Input handle
    void PunchAttackHandle()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // if attack locked → mark this input as invalid

            if (!canAttack)
            {
                isInputStateLock = true;
                return;
            }

            isHolding = true;
            holdTimer = 0f;
            chargedFired = false;
            isInputStateLock = false;
        }

        // While holding
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            OnHoldingAttack();

            Debug.Log($"Loading... {(int)holdTimer:F2}");

            if (holdTimer >= chargeTime && !chargedFired)
            {
                chargedFired = true;
                isHolding = false;

                Debug.Log("CHARGED PUNCH!");
                StartCoroutine(HandPunchAttack());
            }
        }

        // Mouse released
        if (Input.GetMouseButtonUp(0))
        {
            if (isInputStateLock)
            {
                isInputStateLock = false;
                isHolding = false;
                holdTimer = 0f;
                return;
            }
            if (!chargedFired && isHolding && canAttack)
            {
                Debug.Log("Quick tap → Normal Punch");

                StartCoroutine(HandPunchAttack());
            }
            isHolding = false;
            holdTimer = 0f;
        }

    }
    #endregion 
    #region Attack Hold
    void OnHoldingAttack()
    {
        if (isStunned) return;

        var state = animator.GetCurrentAnimatorStateInfo(actionLayer);
        var next = animator.GetNextAnimatorStateInfo(actionLayer);

        if (state.IsName("HandRotate") || next.IsName("HandRotate"))
            return;

        animator.SetLayerWeight(actionLayer, 1f);
        animator.CrossFade("HandRotate", 0.08f, actionLayer);
    }
    #endregion
    #region  Punch Attack
    IEnumerator HandPunchAttack()
    {
        if (!canAttack) yield break;
        if (isStunned) yield break;
        if (isDead) yield break;

        canAttack = false;
        isPunching = true;

        attackBtn.interactable = canAttack;

        //action set to 0
        animator.Play("Empty", actionLayer);
        animator.SetLayerWeight(actionLayer, 0f);

        //Reaction layer set to 1
        animator.SetLayerWeight(reactionLayer, 1);
        animator.Play("HandAttack", reactionLayer);

        yield return new WaitForSeconds(0.5f);
        isPunching = false;
        DoHit();

        //StartCoroutine(AttackCoolDown());
        yield return new WaitForSeconds(handAttackCoolDownTime);
        animator.Play("Empty", reactionLayer);
        animator.SetLayerWeight(reactionLayer, 0f);

        //Reset Attack Time
        yield return new WaitForSeconds(0.15f);
        canAttack = true;
        attackBtn.interactable = canAttack;
        isPunching = false;

        img.color = Color.white;
        // canAttack = true;
    }
    void DoHit()
    {
        Collider[] hits = Physics.OverlapSphere(attackPosition.position, attackRadius);
        foreach (Collider hit in hits)
        {
            if (hit.transform.root == transform.root) continue;

            Debug.Log($"Hit: {hit.name}");
            Vector3 effectSpawn = hit.ClosestPoint(attackPosition.position);
            effectSpawn.y += 1.25f;
            Instantiate(effect, effectSpawn, Quaternion.identity);
            if (hit.attachedRigidbody != null)
            {
                Vector3 dir = (hit.attachedRigidbody.position - transform.position).normalized;
                dir.y += 0.6f;
                IDamageable otherDmg = hit.GetComponentInParent<IDamageable>();

                if (otherDmg != null)
                    otherDmg.TakeDamage(dir, attackForce, transform);
            }
        }
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            count++;
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            count--;
            isGrounded = count > 0;
        }
    }
    bool InAirCheck()
    {
        if (isGrounded)
        {
            inAirTimer = 0f;
            return false;
        }

        if (count != 0) return false;

        inAirTimer += Time.deltaTime;

        if (inAirTimer >= 0.5f)
            return true;

        return false;
    }
    #region Take Damage
    public void TakeDamage(Vector3 dir, float force, Transform attacker = null)
    {
        //can control fasle, rb's velocity = 0, add force dir 
        //  if (isPunching) return;
        if (isDead) return;
        if (isStunned) return;

        ForceInterruptAll();
        canMove = false;
        attackBtn.interactable = true;
        isStunned = true;
        isStateLocked = true;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * force, ForceMode.Impulse);
        playerHelath -= 10;
        healthbarImg.fillAmount = (float)playerHelath / 100;
        animator.SetLayerWeight(actionLayer, 0);
        if (playerHelath <= 0)
        {
            Die();
            return;
        }
        // PlayerAnimationStateUpdate(PlayerState.Attacked, true, 0.2f);
        animator.SetLayerWeight(reactionLayer, 1);
        // animator.Play("Attacked", reactionLayer);
        animator.CrossFade("Attacked_2", 0.2f, reactionLayer);
        StartCoroutine(StunnedBack());
    }

    IEnumerator StunnedBack()
    {
        yield return new WaitForSeconds(1.5f);

        rb.linearVelocity = Vector3.zero;

        isStunned = false;
        isStateLocked = false;
        canMove = true;
        canAttack = true;
        attackBtn.interactable  = true;
        img.color = Color.white;

        animator.Play("Empty", reactionLayer);
        animator.SetLayerWeight(reactionLayer, 0f);

    }


    void Die()
    {
        if (isDead) return; // safety
        isDead = true;

        StopAllCoroutines(); // 🔥 stop stun / attack etc

        canMove = false;
        isStunned = true;
        isStateLocked = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; // optional but good

        PlayerAnimationStateUpdate(PlayerState.Attacked, true, 0.1f);
        animator.Play("Attacked", 0);
        Debug.LogError("Dead  hn");
        StartCoroutine(AfterDie());

    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 effectPos = transform.position;
        effectPos.y += 1;
        Instantiate(deathEffect, effectPos, Quaternion.Euler(-90, 0, 0));
        yield return new WaitForSeconds(2f);
        Instantiate(scareCrow, transform.position, Quaternion.Euler(0, 180, 0));
        //gameObject.SetActive(false);
        Horror_LvL_UIManager.PlayerDeadCheck();


    }

    #endregion
    IEnumerator LevelSelection()
    {
        yield return new WaitForSeconds(3f);
        var handle = Addressables.LoadSceneAsync("LevelSelection", LoadSceneMode.Single);

        while (!handle.IsDone)
        {
            float percent = handle.PercentComplete;

            yield return null;
        }

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Scene load failed");
        }

    }
    #region Click Attack System
    // called when button pressed
    public void AttackPress()
    {
        if (!canAttack)
        {
            isInputStateLock = true;
            return;
        }
        if (isStunned) return;
        isHolding = true;
        holdTimer = 0f;
        chargedFired = false;
        isInputStateLock = false;
    }

    // called while button holding (every frame)
    public void AttackHold()
    {
        if (!isHolding) return;
        if (isStunned) return;

        holdTimer += Time.deltaTime;
        OnHoldingAttack();

        if (holdTimer >= chargeTime && !chargedFired)
        {
            chargedFired = true;
            isHolding = false;
            StartCoroutine(HandPunchAttack());
            img.color = Color.grey;
        }
    }

    // called when button released
    public void AttackRelease()
    {
        if (isInputStateLock)
        {
            isInputStateLock = false;
            isHolding = false;
            holdTimer = 0f;
            return;
        }

        if (!chargedFired && isHolding && canAttack)
        {
            StartCoroutine(HandPunchAttack());
            img.color = Color.grey;
        }

        isHolding = false;
        holdTimer = 0f;
    }
    #endregion
    #region ForceInterruptAll
    void ForceInterruptAll()
    {
        //if(isDead) return;
        StopAllCoroutines();
        Debug.LogWarning(isDead + "Kya hai");
        isPunching = false;
        isHolding = false;
        chargedFired = false;

        //New line---
        isJumping = false;
        isStateLocked = false;
        //---
        canAttack = false; // temporarily block
        Debug.LogError("This is forcedIn");
        // Reset layers
        if (!isDead)
        {
            animator.Play("Empty", reactionLayer);
            animator.SetLayerWeight(reactionLayer, 0f);

        }
        else
        {
            animator.Play("Empty", reactionLayer);
            animator.SetLayerWeight(reactionLayer, 0f);
            animator.Play("Attacked", 0);
            PlayerAnimationStateUpdate(PlayerState.Attacked, true, 0.1f);

        }

    }
    #endregion
    private void OnDrawGizmos()
    {
        if (attackPosition == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
