using UnityEngine;
using System.Collections.Generic;

public class PlayerEnemyTracker : MonoBehaviour
{
    private Transform targetEnemy;
    public float rotationSpeed = 5f;
    private Rigidbody rb;

    private List<Transform> enemiesInRange = new List<Transform>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found! Adding one now...");
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.freezeRotation = true;
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
        Vector3 direction = (targetEnemy.position - transform.position);
        direction.y = 0;
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
