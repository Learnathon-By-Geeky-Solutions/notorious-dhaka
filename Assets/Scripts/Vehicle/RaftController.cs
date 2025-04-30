using UnityEngine;
using UnityEngine.UI;

public class RaftController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 50f;
    public float maxSpeed = 8f;
    public float waterDrag = 1f;

    [Header("Floating Settings")]
    public float floatHeight = 1.5f;
    public Transform waterTransform;

    [Header("References")]
    public GameObject player;
    public Transform raftSeatPoint;
    public GameObject pressEnterUI;

    private Rigidbody rb;
    private bool isPlayerOnRaft = false;
    private bool isNearRaft = false;
    private bool isOnWater = false;

    private float moveInput;
    private float turnInput;

    private Vector3 playerOriginalPosition;
    private Quaternion playerOriginalRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = waterDrag;
        rb.angularDrag = 2f;
        rb.mass = 50f;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        pressEnterUI.SetActive(false);
    }

    void Update()
    {
        if (isNearRaft && !isPlayerOnRaft)
        {
            pressEnterUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return))
                GetOnRaft();
        }
        else if (isPlayerOnRaft && Input.GetKeyDown(KeyCode.Return))
        {
            ExitRaft();
        }

        if (isPlayerOnRaft && isOnWater)
        {
            moveInput = Input.GetAxisRaw("Vertical");
            turnInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            moveInput = 0f;
            turnInput = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isOnWater)
            ApplyBuoyancy();

        if (isPlayerOnRaft && isOnWater)
        {
            Vector3 forwardForce = transform.forward * moveInput * moveSpeed;
            rb.AddForce(forwardForce, ForceMode.Force);

            if (Mathf.Abs(turnInput) > 0.1f)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f));
            }

            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void ApplyBuoyancy()
    {
        float waterLevel = waterTransform.position.y;
        float depth = waterLevel - transform.position.y + floatHeight;

        if (depth > 0f)
        {
            Vector3 uplift = -Physics.gravity * (depth / floatHeight);
            rb.AddForce(uplift, ForceMode.Acceleration);
        }
    }

    void GetOnRaft()
    {
        playerOriginalPosition = player.transform.position;
        playerOriginalRotation = player.transform.rotation;

        player.transform.SetParent(raftSeatPoint);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        var playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            // 🛠️ Set velocity before making it kinematic
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        var playerCol = player.GetComponent<Collider>();
        if (playerCol != null)
            playerCol.enabled = false;

        var movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.SetOnRaft(true);

        isPlayerOnRaft = true;
        pressEnterUI.SetActive(false);
    }

    void ExitRaft()
    {
        player.transform.SetParent(null);
        player.transform.position = transform.position + transform.right * 2f;
        player.transform.rotation = playerOriginalRotation;

        var playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
            playerRb.isKinematic = false;

        var playerCol = player.GetComponent<Collider>();
        if (playerCol != null)
            playerCol.enabled = true;

        var movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.SetOnRaft(false);

        isPlayerOnRaft = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            isOnWater = true;

        if (other.CompareTag("Player"))
            isNearRaft = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            isOnWater = false;

        if (other.CompareTag("Player"))
        {
            isNearRaft = false;
            pressEnterUI.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
