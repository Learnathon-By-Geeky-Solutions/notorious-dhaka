using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private bool isInWater = false;
    public float buoyancyForce = 5f;  // Adjust this for floating effect
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
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Kick");
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }

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
        if (other.CompareTag("Water"))  // Make sure the water object has the "Water" tag
        {
            isInWater = true;
            rb.useGravity = false;  // Disable gravity in water
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset downward velocity
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.useGravity = true;  // Restore normal gravity
        }
    }
}
