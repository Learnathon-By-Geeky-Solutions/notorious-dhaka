using UnityEngine;
using UnityEngine.EventSystems;

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
        rb.MovePosition(new Vector3(targetPosition.x, fixedY, targetPosition.z));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rb.isKinematic = false;
    }

    private Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Vector3 screenPosition = eventData.position;
        screenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;

        return mainCamera.ScreenToWorldPoint(screenPosition);
    }
}
