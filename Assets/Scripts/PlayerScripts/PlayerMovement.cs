using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isPunching = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Punch");
            isPunching = true; 
            Invoke(nameof(ResetPunch), 0.5f); 
        }

     
        if (animator != null && !isPunching) 
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
        }

      
        if (moveDirection != Vector3.zero && !isPunching)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (rb != null && !isPunching) 
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void ResetPunch()
    {
        isPunching = false;
    }
}
