using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxMoveSpeed = 5f;
    public float turnSpeed = 10f;
    public float jumpForce = 7f;
    public AudioSource walkAudio;
    public float walkSpeedThreshold = 0.1f;

    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private bool isInWater = false;
    private bool isGrounded = true;
    private bool isOnRaft = false;

    private Camera mainCam;
    private PlayerSwim swimScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        swimScript = GetComponent<PlayerSwim>();

        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (camObj != null)
            mainCam = camObj.GetComponent<Camera>();

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = true;

        if (walkAudio == null)
            walkAudio = GetComponent<AudioSource>();

        if (walkAudio != null)
        {
            walkAudio.loop = true;
            walkAudio.playOnAwake = false;
        }
    }

    void Update()
    {
        if (isOnRaft) return; // 🚫 Disable all input if on raft

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        if (input.magnitude > 0.1f)
        {
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            moveDirection = (camForward * input.z + camRight * input.x).normalized;

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        if (walkAudio != null && !isInWater)
        {
            bool isWalkingInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                                  Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                                  Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                                  Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow);

            if (isWalkingInput && !walkAudio.isPlaying)
                walkAudio.Play();
            else if (!isWalkingInput && walkAudio.isPlaying)
                walkAudio.Stop();
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Kick");
        }

        // Jump only when grounded and not in water or raft
        if (Input.GetKeyDown(KeyCode.Space) && !isInWater)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (isOnRaft) return; // 🚫 No movement update on raft

        Vector3 movement = moveDirection * maxMoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            animator?.SetTrigger("JumpTrigger");
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            isGrounded = false;
            walkAudio?.Stop();

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (swimScript != null)
                swimScript.StartSwimming();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            isGrounded = true;

            if (swimScript != null)
                swimScript.StopSwimming();
        }
    }

    // 🔄 Called from RaftController to control input/physics
    public void SetOnRaft(bool value)
    {
        isOnRaft = value;
    }
}
