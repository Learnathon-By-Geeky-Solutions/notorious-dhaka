using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    public Transform target;
    void Start()
    {

    }
    void Update()
    {
        transform.position = target.position;
    }
}
