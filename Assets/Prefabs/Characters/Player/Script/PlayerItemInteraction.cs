using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace PlayerInteract
{
    public class PlayerItemInteraction : MonoBehaviour
    {
        public Inventory storage;
        public GameObject canvas;

        [Header("Sound")]
        public AudioSource collectAudioSource;
        public AudioClip collectSound;

        public static PlayerItemInteraction Instance; // for global access to play sound

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            storage = FindObjectOfType<Inventory>();
            canvas.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                canvas.SetActive(!canvas.activeSelf);
            }
        }

        public void PlayCollectSound()
        {
            if (collectAudioSource != null && collectSound != null)
            {
                collectAudioSource.PlayOneShot(collectSound);
            }
        }
    }
}
