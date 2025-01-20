using System.Collections;
using UnityEngine;

namespace CameraBehaves
{
    public class CameraMovement : MonoBehaviour
    {
        private Transform target; // Private field

        // Public property for encapsulation
        public Transform Target
        {
            get => target;
            set => target = value;
        }

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
}
