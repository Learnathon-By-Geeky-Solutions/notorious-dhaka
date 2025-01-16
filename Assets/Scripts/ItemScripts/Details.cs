using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
