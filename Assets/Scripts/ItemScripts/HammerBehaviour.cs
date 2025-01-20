using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterBehaves;

namespace ItemBehaves
{
    public class HammerBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.GetComponent<FrogBehaviour>() != null) // Ensure it's a FrogBehaviour enemy
                {
                    FrogBehaviour.TakeDamage(other.gameObject, 25f); // Correctly access the static method
                }
            }
        }
    }
}
