using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        // This method is intentionally left empty.
        // It serves as a placeholder for future initialization logic, if required.
    }
    void Update()
    {
        if (target != null)
            transform.position = target.position;
        else
            Debug.Log("Game Over!");
    }
}
