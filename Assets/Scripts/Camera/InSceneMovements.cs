using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InSceneMovements : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(-10f, 15f, -10f);
    [SerializeField] private float followSpeed = 5f;

    private Transform target;
    public Transform Target
    {
        get => target;
        set => target = value;
    }

    void Start()
    {
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
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
        else
        {
            Debug.Log("Game Over!");
        }
    }
}
