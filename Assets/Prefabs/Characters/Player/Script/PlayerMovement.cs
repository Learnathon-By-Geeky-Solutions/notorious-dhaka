using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxMoveSpeed = 5f;
    public float turnSpeed = 10f;

    [Header("Jumping")]
    public float jumpForce = 7f;

    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private bool isInWater = false;
    public float buoyancyForce = 5f;
    public float verticalSwimSpeed = 5f;

    private Camera mainCam;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Now correctly search for the MainCamera tag
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (camObj != null)
        {
            mainCam = camObj.GetComponent<Camera>();
        }

        if (mainCam == null)
        {
            Debug.LogError("MainCamera not found or missing Camera component. Make sure it's tagged correctly and has a Camera component.");
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        if (input.magnitude > 0.1f)
        {
            // Convert input to camera space
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

        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Kick");
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isInWater)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = moveDirection * maxMoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        if (isInWater)
        {
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);

            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector3(rb.velocity.x, verticalSwimSpeed, rb.velocity.z);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.velocity = new Vector3(rb.velocity.x, -verticalSwimSpeed, rb.velocity.z);
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        if (animator != null)
        {
            animator.SetTrigger("JumpTrigger");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.useGravity = true;
        }
    }
}
