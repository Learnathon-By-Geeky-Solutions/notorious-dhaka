using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        private float speed = 10f;
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // This method is intentionally left empty.
            // It serves as a placeholder for future initialization logic, if required.
        }
        void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

}