using UnityEngine;

public class PlayerSwim : MonoBehaviour
{
    [Header("Swimming Settings")]
    public float swimSpeed = 3f;
    public float rotationSpeed = 5f;
    public float floatingForce = 1f;
    public float waterDrag = 2f;
    public float targetDepthOffset = -1.2f;

    private Rigidbody rb;
    private Animator anim;
    private float waterSurfaceY;
    private bool isSwimming = false;
    private bool isMoving = false;

    private static readonly int StartSwimmingTrigger = Animator.StringToHash("StartSwimming");
    private static readonly int SwimTrigger = Animator.StringToHash("Swim");
    private static readonly int StopSwimmingTrigger = Animator.StringToHash("StopSwimming");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isSwimming)
        {
            HandleSwimming();
        }
    }

    public void StartSwimming()
    {
        if (isSwimming) return;

        isSwimming = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.drag = waterDrag;

        anim.SetTrigger(StartSwimmingTrigger);
        waterSurfaceY = transform.position.y;
    }

    public void StopSwimming()
    {
        if (!isSwimming) return;

        isSwimming = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.drag = 0;

        anim.ResetTrigger(StartSwimmingTrigger);
        anim.ResetTrigger(SwimTrigger);
        anim.SetTrigger(StopSwimmingTrigger);
    }

    private void HandleSwimming()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ❌ Completely ignore Space / LeftControl to block up/down
        Vector3 swimDirection = new Vector3(horizontal, 0f, vertical).normalized;
        isMoving = swimDirection.magnitude > 0.1f;

        if (isMoving)
        {
            Vector3 velocity = swimDirection * swimSpeed;
            velocity.y = rb.velocity.y; // Preserve buoyancy
            rb.velocity = velocity;

            anim.SetTrigger(SwimTrigger);

            Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            anim.ResetTrigger(SwimTrigger);
            anim.SetTrigger(StartSwimmingTrigger);
        }

        // ✅ Keep floating at target depth
        float targetDepth = waterSurfaceY + targetDepthOffset;
        float depthDifference = targetDepth - transform.position.y;
        rb.AddForce(Vector3.up * depthDifference * floatingForce, ForceMode.Acceleration);
    }
}
