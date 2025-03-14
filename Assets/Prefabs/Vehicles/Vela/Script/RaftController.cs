using UnityEngine;

public class RaftController : MonoBehaviour
{
    public float buoyancyForce = 10f;  
    public float waterDrag = 0.95f;    
    public float acceleration = 5f;    
    public float maxSpeed = 8f;        
    public float turnSpeed = 50f;      
    public float waterLevel = 0f;      
    public float stabilityForce = 5f;  

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
           
            rb.AddForce(Vector3.up * buoyancyForce * depth, ForceMode.Acceleration);

            
            Vector3 stability = -rb.angularVelocity * stabilityForce;
            rb.AddTorque(stability, ForceMode.Acceleration);
        }
        else
        {
            
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
