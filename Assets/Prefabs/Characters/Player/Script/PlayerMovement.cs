using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxMoveSpeed; // Increased for larger step coverage
    public float acceleration; // Increased for quicker speed buildup
    public float deceleration;
    public float turnSpeed;

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

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude > 0)
        {
            // Smoothly rotate towards movement direction
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }

        // Adjust animation speed based on movement
        if (animator != null)
        {
            animator.SetFloat("Speed", currentVelocity.magnitude / maxMoveSpeed * 2f); // Adjusted animation sync
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Kick");
        }
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            // Apply velocity directly for better response
            currentVelocity = moveDirection * maxMoveSpeed;
        }
        else
        {
            // Decelerate smoothly when no input is given
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        }

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

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
