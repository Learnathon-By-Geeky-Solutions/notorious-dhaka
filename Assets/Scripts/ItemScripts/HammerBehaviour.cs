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
                if (other.GetComponent<FrogBehaviour>() != null)
                {
                    FrogBehaviour.TakeDamage(other.gameObject, 25f);
                }
            }
        }
    }
}
