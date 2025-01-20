using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace ItemZone
{
    public class Details : MonoBehaviour
    {
        public Items item;
        public Inventory storage;
        void Start()
        {
            storage = FindObjectOfType<Inventory>();
        }
        void Update()
        {
            // This method is intentionally left empty.
            // It serves as a placeholder for future initialization logic, if required.
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (item != null && storage != null)
                {
                    storage.AddItem(item);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("Item or Storage is null!");
                }
            }
        }
    }

}