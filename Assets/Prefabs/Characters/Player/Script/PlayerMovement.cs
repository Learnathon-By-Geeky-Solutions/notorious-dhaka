using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxMoveSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float turnSpeed = 10f;

    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 currentVelocity = Vector3.zero;

    private bool isInWater = false;
    public float buoyancyForce = 5f;
    public float verticalSwimSpeed = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude > 0)
        {
            // Smooth rotation towards movement direction
            Quaternion toRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }

        // Update animation speed parameter
        if (animator != null)
        {
            animator.SetFloat("Speed", currentVelocity.magnitude / maxMoveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Kick");
        }

        moveDirection = inputDirection; // Store movement input
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            // Accelerate towards max speed
            currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * maxMoveSpeed, Time.fixedDeltaTime * acceleration);
        }
        else
        {
            // Decelerate smoothly
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        }

        // Apply movement with inertia
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

        if (isInWater)
        {
            // Simulate buoyancy
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);

            // Allow vertical swimming
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset downward velocity
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
