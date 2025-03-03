using System.Collections;
using UnityEngine;

namespace CameraBehaves
{
    public class CameraMovement : MonoBehaviour
    {
        private Transform target;
        public Transform Target
        {
            get => target;
            set => target = value;
        }

        void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Target = player.transform;
                }
                else
                {
                    Debug.LogError("Player not found! Ensure the Player GameObject has the 'Player' tag.");
                }
            }
        }

        void Update()
        {
            if (target != null)
            {
                transform.position = target.position;
            }
            else
            {
                Debug.Log("Game Over!");
            }
        }
    }
}
