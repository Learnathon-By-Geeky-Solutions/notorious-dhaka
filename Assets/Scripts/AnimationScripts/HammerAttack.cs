using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : MonoBehaviour
{
    public Animator animator; // For the animation
    public Camera playerCamera; // The player's camera
    public LayerMask enemyLayer; // To detect enemies
    public float attackRange = 2f; // How far the hammer can reach

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseButtonPressed");
            // Send out a "ray" from the camera to where the mouse is pointing
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an enemy
            if (Physics.Raycast(ray, out hit, attackRange, enemyLayer))
            {
                Debug.Log("Raycast Hitting");
                // Play the hammer attack animation
                animator.SetTrigger("AttackTrigger");
            }
        }
    }
}
