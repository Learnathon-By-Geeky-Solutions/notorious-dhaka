using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Assign the player in Inspector

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}