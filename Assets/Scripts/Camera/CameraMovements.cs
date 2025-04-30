using UnityEngine;

namespace CameraBehaves
{
    public class CameraMovement : MonoBehaviour
    {
        private Transform target;
        private Vector3 basePosition;

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

        void LateUpdate() // ✅ Use LateUpdate to allow shake to apply after movement
        {
            if (target != null)
            {
                basePosition = target.position;
                transform.position = basePosition;
            }
        }

        public Vector3 GetBasePosition()
        {
            return basePosition;
        }
    }
}
