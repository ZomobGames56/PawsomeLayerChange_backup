using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HY_Player_Control : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Joystick joystick;// JoyStick Refrence Given in the canvas.
    [SerializeField]
    Rigidbody rb;//Player Rigdbody
    Button btn;
    Vector3 move;
    [SerializeField]
    float moveSpeed = 10f, force = 7f, defaultSpeed = 0.97f, onSliderSpeed = 2.0f,
        waitForSec = 0.5f, transformMoveSpeed = 8f, logMoveSpeed = 7f;//Jump Force
    [SerializeField]
    public Animator animator;
    [SerializeField]
    bool isGrounded, isDashing;
    [SerializeField]
    Transform cam;
    bool jumpbtnPressed = false;
    [SerializeField]
    Transform spawnPoint, firstSp, secondSp, thirdSp, fourthSp;
    [SerializeField]
    bool inAir;
    [SerializeField]
    GameObject effect;

    //[SerializeField]
    //float lastTapTime = 0f, doubleTapThreshold = 0.3f;
    [SerializeField]
    float dashDuration = 0.5f, dashSpeed = 10f;
    Vector3 playerScale;
    [SerializeField]
    float scale = 0.75f;
    [SerializeField]
    public static bool canControl;
    float inAirTime;
    RaycastHit hit;

    bool isCalled;
    public bool rigidBodyControl, transformControl;
    [SerializeField]
    ParticleSystem dustEffect;

    [SerializeField]
    AudioClip jumpSound, fallInWater, collideSound;
    bool collideToWater;
    public GameObject dummyScreen;
    [SerializeField] HY_CameraControl camControl;
    public bool testBool;
    public int count;
    Coroutine jumpRoutine;
    [SerializeField]
    LayerMask layer;
    [SerializeField]
    Transform head;
    int mask;
    public bool obstacleCollide = false;
    [SerializeField]
    int outOfBoundVal = -51;

    // PLATFORM SYSTEM
    private Rigidbody currentPlatformRb;
    private Vector3 lastPlatformPos;
    private Quaternion lastPlatformRot;

    #endregion 
    void Start()
    {
        collideToWater = false;

        isCalled = false;
        testBool = canControl;
        canControl = true;

        rb = GetComponent<Rigidbody>();

        isGrounded = false;

        animator = GetComponent<Animator>();

        if (rb != null && rigidBodyControl)
        {

            rb.position = spawnPoint.position;
            rb.rotation = spawnPoint.rotation;

        }

        //rb.MovePosition(spawnPoint.position);
        playerScale = new Vector3(scale, scale, scale);

        transform.localScale = playerScale;
        //  rigidBodyControl = true;
        //  transformControl = false;
        jumpbtnPressed = false;

    }
    // Update is called once per frame

    void Update()
    {
        testBool = canControl;
        PlayerOutOfBounds();
        if (!isGrounded && rb.linearVelocity.y < -0.1f)
        {
            animator.SetBool("Hanging", true);
        }
        else
        {
            animator.SetBool("Hanging", false);
        }

        if (canControl == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MobileJumpBtn();
            }
            CanAniamte();
        }
    }

    private void FixedUpdate()
    {
        if (canControl == true)
        {
            PlayerMovement();
            ApplyPlatformVelocity();
        }
    }
    void LateUpdate()
    {
        if (count > 0)
            isGrounded = true;
        else
            isGrounded = false;
    }
    void HangingAnimation()
    {
        animator.SetBool("Hanging", true);
    }// hanging animation true..

    public void PlayerMovement()
    {
        if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer && canControl
            || Application.platform == RuntimePlatform.WindowsEditor))
        {
            #region
            ////Debug.Log("Mobile");
            //joystick.gameObject.SetActive(true);
            //move = (cam.right * joystick.Horizontal +
            //       cam.forward * joystick.Vertical).normalized;
            //move.y = 0f;
            //if (transformControl)
            //{
            //    transform.position += move * moveSpeed * Time.deltaTime;
            //   // Debug.Log("Transform one is calling");
            //}
            //if (rigidBodyControl)
            //{
            //    if (rb != null)
            //    {
            //        rb.MovePosition(transform.position + move * moveSpeed * Time.fixedDeltaTime);
            //       // Debug.Log("rigid one is calling");

            //    }
            //}

            //if (move.magnitude != 0)
            //{
            //  //  dustEffect.Play();
            //    Rotate();
            //}
            ////if (move.magnitude == 0)
            ////{
            ////    dustEffect.Stop();
            ////}
            #endregion
            joystick.gameObject.SetActive(true);

            Vector3 camForwad = cam.forward;

            Vector3 camRight = cam.right;
            camForwad.y = 0f; camRight.y = 0f;
            camForwad.Normalize(); camRight.Normalize();
            // float h = Input.GetAxis("Horizontal");
            // float v = Input.GetAxis("Vertical");
            move = camRight * joystick.Horizontal + camForwad * joystick.Vertical;

            move = Vector3.ClampMagnitude(move, 1f);

            move.y = 0f;

            // for camera rotation.!!!!!!
            if (camControl != null && canControl)
            {
                camControl.playerMoveDir = move;
            }

            if (transformControl)
            {
                transform.position += move * moveSpeed * Time.deltaTime;
            }
            if (rigidBodyControl)
            {
                #region
                //if (rb != null)
                //{

                //    Vector3 velocity = rb.linearVelocity;
                //    if (move.sqrMagnitude > 0.01f)
                //    {
                //        velocity.x = move.x * moveSpeed;
                //        velocity.z = move.z * moveSpeed;
                //    }
                //    else
                //    {
                //        // Hard stop when input released
                //        velocity.x = 0f;
                //        velocity.z = 0f;
                //    }

                //    rb.linearVelocity = velocity;
                //    // rb.velocity = move*moveSpeed;


                //    // animator.SetFloat("Run",rb.linearVelocity.magnitude);
                //    //Debug.Log("rigid one is calling");

                //}
                #endregion Old Input 
                if (rb != null)
                {
                    Vector3 velocity = rb.linearVelocity;

                    if (move.sqrMagnitude > 0.01f)
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
                        velocity.x = 0f;
                        velocity.z = 0f;
                    }

                    rb.linearVelocity = velocity;
                }
            }
            if (move.magnitude != 0)
            {
                Rotate();
                // dustEffect.Play();
            }
        }

        else if ((Application.platform == RuntimePlatform.WindowsPlayer) && canControl)
        {
            joystick.gameObject.SetActive(false);
            Vector3 camForwad = cam.forward;
            Vector3 camRight = cam.right;
            camForwad.y = 0f;
            camRight.y = 0f;
            camForwad.Normalize();
            camRight.Normalize();
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            move = camRight * h + camForwad * v;
            move = Vector3.ClampMagnitude(move, 1f);

            // move.Normalize();

            move.y = 0f;
            if (transformControl)
            {
                transform.position += move * moveSpeed * Time.deltaTime;
                Debug.Log("Transform one is calling");
            }
            if (rigidBodyControl)
            {
                if (rb != null)
                {
                    Vector3 velocity = rb.linearVelocity;

                    if (move.sqrMagnitude > 0.01f)
                    {
                        Vector3 moveDir = move;

                        if (Physics.Raycast(transform.position, moveDir, out hit, 0.6f, ~0, QueryTriggerInteraction.Ignore))
                        {
                            // 🔥 Remove movement into wall
                            moveDir = Vector3.ProjectOnPlane(moveDir, hit.normal);
                        }

                        velocity.x = moveDir.x * moveSpeed;
                        velocity.z = moveDir.z * moveSpeed;
                    }
                    else
                    {
                        velocity.x = 0f;
                        velocity.z = 0f;
                    }

                    rb.linearVelocity = velocity;

                }
            }
            if (move.magnitude != 0)
            {
                Rotate();
                // dustEffect.Play();
            }
        }

    }// joy stick movment.
    public void MobileJumpBtn()
    {
        if (isGrounded && !jumpbtnPressed)
        {
            if (jumpRoutine != null)
                StopCoroutine(jumpRoutine);

            jumpbtnPressed = true;

            Debug.Log("Jump Calling");
            animator.SetBool("Jump", true);
            animator.SetBool("Hanging", false);

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);

            isGrounded = false;


            jumpRoutine = StartCoroutine(JumpUp());
        }
    }
    IEnumerator JumpUp()
    {
        yield return new WaitForSeconds(0.2f);

        animator.SetBool("Jump", false);
        Debug.Log("Jumping");
        while (!isGrounded)
        {
            animator.SetBool("Hanging", true);
            yield return null;
        }
        animator.SetBool("Hanging", false);
        jumpbtnPressed = false;

    }
    IEnumerator Dash()
    {
        //dash animation.
        animator.SetBool("Dash", true);
        isDashing = true;
        float timer = 0;
        while (timer < dashDuration)
        {
            rb.MovePosition(Vector3.forward * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            print("Dash Animation called");
            yield return null;
        }
        isDashing = false;
        //dash animation stop
        animator.SetBool("Dash", false);
    }//dash animation....//Not in use yet...............
    public void CanAniamte()
    {

        if (move.magnitude != 0)
        {
            animator.SetFloat("Run", move.magnitude);
        }
        else
        {
            animator.SetFloat("Run", 0);
        }

    }

    public void Rotate()
    {
        Quaternion rot = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot,
            720f * Time.deltaTime);
    }

    public void OnCollisionStay(Collision collision)
    {

        if (collision.transform.tag == "Slider")
        {
            moveSpeed = onSliderSpeed;
            isDashing = true;
            animator.SetBool("Jump", false);
            animator.SetBool("Dash", true);
            animator.SetTrigger("Dashing");
            animator.SetBool("Hanging", false);
            isGrounded = true;
            jumpbtnPressed = false;
            inAir = false;

        }

        if (collision.gameObject.CompareTag("Log"))
        {
            if (currentPlatformRb != collision.rigidbody)
            {
                currentPlatformRb = collision.rigidbody;
                lastPlatformPos = currentPlatformRb.position;
                lastPlatformRot = currentPlatformRb.rotation;
            }
        }
        //if (collision.transform.tag == "Ground")
        //{
        //    animator.SetBool("Hanging", false);
        //}

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            HY_PlayerRagdollActive.instance.OnObstacleCollide();
            print("Player");
        }
        if (collision.transform.tag == "Shovel")
        {
            HY_PlayerRagdollActive.instance.OnShovelHit();
        }
        //if (collision.transform.tag == "Water" && !collideToWater)
        //{
        //    collideToWater = true;
        //    OnCollideWater();
        //}
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Log"))
        {
            currentPlatformRb = null;
        }
    }
    void PlayerOutOfBounds()
    {
        if (transform.position.y <= outOfBoundVal && !isCalled)
        {
            Debug.Log("Player out of bound");
            // gameObject.SetActive(false
            isCalled = true;
            // canControl = false;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 500f);
            //rb.isKinematic = true;
            StartCoroutine(SpawnWait());


        }
    }
    public IEnumerator SpawnWait()
    {
        rb.isKinematic = true;

        yield return new WaitForSeconds(waitForSec);
        // Stop physics
        //rb.linearVelocity = Vector3.zero;


        // Teleport correctly
        rb.position = spawnPoint.position;
        rb.rotation = spawnPoint.rotation;
        transform.rotation = spawnPoint.rotation;
        //rb.isKinematic = false;
        // Reset scale instantly
        transform.localScale = playerScale;
        // Reset states
        canControl = true;
        isCalled = false;
        isGrounded = true;
        inAir = false;
        jumpbtnPressed = false;
        isDashing = false;
        moveSpeed = defaultSpeed;

        animator.SetBool("Dash", false);
        animator.SetBool("Hanging", false);
        animator.SetBool("Jump", false);

        if (camControl != null)
        {
            camControl.currentX = 360f;
        }
        rb.isKinematic = false;
        HY_CameraControl.CameraSnapToPlayerDirection(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "FirstSp":
                spawnPoint = firstSp;
                break;
            case "SecondSp":
                spawnPoint = secondSp;
                break;
            case "ThirdSp":
                spawnPoint = thirdSp;
                break;
            case "FourthSp":
                spawnPoint = fourthSp;
                break;
        }
        if (other.CompareTag("Ground") || other.CompareTag("Log"))
        {
            count++;
            isGrounded = true;
            isCalled = false;
            collideToWater = false;

            animator.SetBool("Jump", false);
            animator.SetBool("Hanging", false);
            inAir = false;
            jumpbtnPressed = false;
            moveSpeed = defaultSpeed;
            animator.SetBool("Dash", false);
            isDashing = false;
            Debug.Log($"IsGround: {isGrounded}");


        }
    }
    Vector3 GetPlatformVelocity()
    {
        if (currentPlatformRb == null) return Vector3.zero;

        Vector3 deltaPlatformPos = currentPlatformRb.position - lastPlatformPos;

        Quaternion deltaRot = currentPlatformRb.rotation * Quaternion.Inverse(lastPlatformRot);

        Vector3 relativePos = rb.position - currentPlatformRb.position;
        Vector3 rotatedPos = deltaRot * relativePos;

        Vector3 rotationMove = rotatedPos - relativePos;

        lastPlatformPos = currentPlatformRb.position;
        lastPlatformRot = currentPlatformRb.rotation;

        return (deltaPlatformPos + rotationMove) / Time.fixedDeltaTime;
    }
    void ApplyPlatformVelocity()
    {
        Vector3 platformVel = GetPlatformVelocity();

        Vector3 velocity = rb.linearVelocity;

        velocity += new Vector3(platformVel.x, 0, platformVel.z);

        rb.linearVelocity = velocity;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Log"))
        {
                count--;
                // 🔥 clamp count (VERY IMPORTANT)
                if (count < 0) count = 0;

                isGrounded = count > 0;

                if (!isGrounded)
                {
                    inAir = true; // 🔥 SET PROPERLY
                }
        }
    }

    void OnDrawGizmos()
    {
        if (head == null) return;

        Gizmos.color = Color.red;

        // Draw movement direction ray
        Vector3 moveDir = move;
        Gizmos.DrawRay(head.position, moveDir.normalized * 0.6f);

        // Draw hit sphere (optional)
        if (Physics.Raycast(head.position, moveDir, out RaycastHit hit, 0.6f, ~0, QueryTriggerInteraction.Ignore))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 0.05f);

            // Draw normal
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(hit.point, hit.normal * 0.3f);
        }
    }
}
