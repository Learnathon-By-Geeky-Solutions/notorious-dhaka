using UnityEngine;
using System.Collections.Generic;

public class PlayerEnemyTracker : MonoBehaviour
{
    private Transform targetEnemy;
    public float rotationSpeed = 5f;
    private Rigidbody rb; // Rigidbody reference

    private List<Transform> enemiesInRange = new List<Transform>();

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found! Adding one now...");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Ensure proper Rigidbody settings
        rb.useGravity = true;
        rb.isKinematic = false; // Allow physics to work
        rb.freezeRotation = true; // Prevent unwanted rotation
    }

    void Update()
    {
        if (targetEnemy != null)
        {
            FaceEnemy();
        }
    }

    void FaceEnemy()
    {
        // Get direction to enemy but ignore Y-axis to prevent tilting up/down
        Vector3 direction = (targetEnemy.position - transform.position);
        direction.y = 0; // Ignore height differences

        // Rotate smoothly towards the enemy
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
            UpdateTargetEnemy();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
            UpdateTargetEnemy();
        }
    }

    void UpdateTargetEnemy()
    {
        if (enemiesInRange.Count == 0)
        {
            targetEnemy = null;
            return;
        }

        // Find the closest enemy
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        targetEnemy = closestEnemy;
    }
}
