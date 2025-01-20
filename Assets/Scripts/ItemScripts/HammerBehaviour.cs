using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            FrogBehaviour frog = other.GetComponent<FrogBehaviour>();
            if (frog != null)
            {
                frog.TakeDamage(25f);
            }
        }
    }
}
