using UnityEngine;

public class RaftController : MonoBehaviour
{
    public float buoyancyForce = 10f;  // Controls how strongly the raft floats
    public float waterDrag = 0.95f;    // Reduces movement in water
    public float acceleration = 5f;    // How fast the raft moves
    public float maxSpeed = 8f;        // Maximum forward speed
    public float turnSpeed = 50f;      // Rotation speed
    public float waterLevel = 0f;      // Adjust this to match your actual water height
    public float stabilityForce = 5f;  // Helps keep the raft level

    private Rigidbody rb;
    private bool isInWater = false;
    private bool isControllable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 1f;
        rb.angularDrag = 2f;
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            ApplyBuoyancy();
            if (isControllable)
            {
                MoveRaft();
            }
            ApplyWaterResistance();
        }
    }

    void ApplyBuoyancy()
    {
        float depth = waterLevel - transform.position.y;

        if (depth > 0)
        {
            // ✅ Apply buoyancy force when below water
            rb.AddForce(Vector3.up * buoyancyForce * depth, ForceMode.Acceleration);

            // ✅ Add stability to prevent excessive tilting
            Vector3 stability = -rb.angularVelocity * stabilityForce;
            rb.AddTorque(stability, ForceMode.Acceleration);
        }
        else
        {
            // ✅ Simulate gravity if above water (so the raft doesn't float in the air)
            rb.AddForce(Vector3.down * buoyancyForce * 0.5f, ForceMode.Acceleration);
        }
    }

    void MoveRaft()
    {
        if (Input.GetKey(KeyCode.W) && rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
        }

        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(Vector3.up * turn * turnSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    void ApplyWaterResistance()
    {
        // ✅ Apply smooth water drag
        rb.velocity *= waterDrag;
        rb.angularVelocity *= waterDrag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.useGravity = false;
            rb.drag = 2f;
            rb.angularDrag = 3f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.useGravity = true;
            rb.drag = 1f;
            rb.angularDrag = 2f;
        }
    }

    public void EnableControl(bool enable)
    {
        isControllable = enable;
    }
}
