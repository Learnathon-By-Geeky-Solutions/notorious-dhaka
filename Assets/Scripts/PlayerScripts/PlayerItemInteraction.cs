using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
public class PlayerItemInteraction : MonoBehaviour
{
    public Inventory storage;
    public GameObject canvas;
    void Start()
    {
        storage = FindObjectOfType<Inventory>();
        canvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }
}
