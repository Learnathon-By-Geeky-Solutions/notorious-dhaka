using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RivanDestruction : MonoBehaviour
{
    public float holdTime = 5f;
    public KeyCode holdKey = KeyCode.X;
    public string targetTag = "Destructible";
    public GameObject uiPromptPrefab;
    public Transform uiCanvas; // Drag your Canvas here in the Inspector
    public Animator rivanAnimator; // Assign Rivan's animator

    private GameObject currentTarget;
    private float holdTimer = 0f;
    private bool isInRange = false;
    private GameObject uiPromptInstance;
    private bool isDestroying = false;

    void Update()
    {
        if (isInRange && currentTarget != null && !isDestroying)
        {
            if (Input.GetKey(holdKey))
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= holdTime)
                {
                    StartCoroutine(TriggerRivanDestruction());
                    isDestroying = true;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            currentTarget = collision.gameObject;
            isInRange = true;

            if (uiPromptPrefab != null && uiPromptInstance == null && uiCanvas != null)
            {
                uiPromptInstance = Instantiate(uiPromptPrefab, uiCanvas);

                TextMeshProUGUI textComponent = uiPromptInstance.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = "Hold X to destroy";
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI not found in uiPromptPrefab.");
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            ResetHold();
        }
    }

    IEnumerator TriggerRivanDestruction()
    {
        if (rivanAnimator != null)
        {
            rivanAnimator.SetTrigger("Destruct");
        }

        // Default delay fallback
        float delay = 1f;

        DestructibleObject destructible = null;

        if (currentTarget != null)
        {
            destructible = currentTarget.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                delay = destructible.destructionDelay;
            }
        }

        // Wait for animation/destruction delay
        yield return new WaitForSeconds(delay);

        // After delay, destroy the object
        if (currentTarget != null)
        {
            Vector3 spawnPos = currentTarget.transform.position + Vector3.up * 0.5f;

            if (destructible != null)
            {
                if (destructible.spawnPoint != null)
                    spawnPos = destructible.spawnPoint.position;

                foreach (GameObject obj in destructible.objectsToSpawn)
                {
                    Instantiate(obj, spawnPos, Quaternion.identity);
                }
            }

            Destroy(currentTarget);
        }

        ResetHold();
    }



    void ResetHold()
    {
        holdTimer = 0f;
        isInRange = false;
        currentTarget = null;
        isDestroying = false;

        if (uiPromptInstance != null)
        {
            Destroy(uiPromptInstance);
            uiPromptInstance = null;
        }
    }
}
