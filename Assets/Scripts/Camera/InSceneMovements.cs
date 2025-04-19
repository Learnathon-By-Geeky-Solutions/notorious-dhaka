using System.Collections;
using UnityEngine;

public class InSceneMovements : MonoBehaviour
{
    [SerializeField] private float followSpeed = 5f;

    private Transform target;
    private Vector3 initialOffset;

    public Transform Target
    {
        get => target;
        set => target = value;
    }

    void Start()
    {
        // Find and assign the player if not already set
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Target = player.transform;
            }
            else
            {
                Debug.LogError("Player not found! Ensure the Player GameObject has the 'Player' tag.");
                return;
            }
        }

        // Calculate the initial offset between camera and player
        initialOffset = transform.position - target.position;

        // Set the rotation to your isometric orthographic angle
        transform.rotation = Quaternion.Euler(50f, -50f, 0f);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow player with offset
            Vector3 desiredPosition = target.position + initialOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Target is missing in InSceneMovements.");
        }
    }
}
