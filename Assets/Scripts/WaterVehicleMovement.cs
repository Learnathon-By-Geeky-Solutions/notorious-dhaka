using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVehicleMovement : MonoBehaviour
{
    public float acceleration = 5f;   // How fast the vehicle speeds up
    public float maxSpeed = 10f;      // Maximum movement speed
    public float turnSpeed = 50f;     // Turning speed
    public float dragFactor = 0.98f;  // Water resistance effect

    private Rigidbody rb;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 1f; // Add water resistance
        rb.angularDrag = 2f; // Smooth turning
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveVehicle();
        }
        ApplyWaterDrag(); // Simulate water friction
    }

    void MoveVehicle()
    {
        // Apply force for forward movement
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
        }

        // Turning the vehicle
        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(Vector3.up * turn * turnSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    void ApplyWaterDrag()
    {
        // Simulate water resistance when not accelerating
        rb.velocity *= dragFactor;
    }

    public void SetMoving(bool state)
    {
        isMoving = state;
    }
}
