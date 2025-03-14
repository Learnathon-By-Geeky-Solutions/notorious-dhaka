using UnityEngine;
using UnityEngine.EventSystems;

namespace ObjectDrag
{
    public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Camera mainCamera;
        private Rigidbody rb;
        private Vector3 offset;
        private float fixedY;

        void Start()
        {
            mainCamera = Camera.main;
            rb = GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogError("Rigidbody component is missing. Please add it to the object.");
            }

            fixedY = transform.position.y;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            offset = transform.position - GetMouseWorldPosition(eventData);
            rb.isKinematic = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 targetPosition = GetMouseWorldPosition(eventData) + offset;

            targetPosition.y = fixedY;

            rb.MovePosition(targetPosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            rb.isKinematic = false;
        }

        private Vector3 GetMouseWorldPosition(PointerEventData eventData)
        {
            Ray ray = mainCamera.ScreenPointToRay(eventData.position);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));

            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return transform.position; 
        }
    }
}
