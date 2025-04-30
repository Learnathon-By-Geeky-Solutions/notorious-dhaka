using UnityEngine;
using Scriptables;
using ObjectDrag;

public class PlaceableSpawner : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask placementLayer;
    public LayerMask selectableLayer;

    private Items itemToPlace;
    private GameObject previewObject;
    private GameObject selectedObject;

    private Quaternion previewRotation = Quaternion.identity;
    private Vector3 originalScale = Vector3.one;
    private bool isPlacing = false;

    public void StartPlacing(Items item)
    {
        itemToPlace = item;

        if (itemToPlace == null || itemToPlace.prefab == null)
        {
            Debug.LogWarning("Missing item or prefab.");
            return;
        }

        // Instantiate object at camera center or 2 meters in front
        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 2f;
        previewObject = Instantiate(itemToPlace.prefab, spawnPos, Quaternion.identity);
        previewObject.name = itemToPlace.itemName;

        // Apply scale
        originalScale = itemToPlace.prefab.transform.localScale;
        previewObject.transform.localScale = originalScale;

        // Ensure collider exists
        if (!previewObject.GetComponentInChildren<Collider>())
        {
            previewObject.AddComponent<BoxCollider>();
        }

        // Add DragNDrop only if not present
        if (!previewObject.TryGetComponent<DragNDrop>(out _))
        {
            previewObject.AddComponent<DragNDrop>();
        }

        // Make it selectable
        previewObject.layer = LayerMask.NameToLayer("Selectable");

        // Finalize placement
        itemToPlace = null;
        previewObject = null;
        isPlacing = false;

        Debug.Log("Object placed and ready for interaction.");
    }

    void Update()
    {
        HandleMouseClick();
        HandleRotation();
    }

    private void HandleMouseClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, selectableLayer))
        {
            GameObject clicked = hit.collider.gameObject;

            // Toggle selection
            if (selectedObject == clicked)
            {
                selectedObject = null;
                Debug.Log("Deselected object.");
            }
            else
            {
                selectedObject = clicked;
                Debug.Log($"Selected object: {selectedObject.name}");
            }
        }
        else
        {
            selectedObject = null;
        }
    }

    private void HandleRotation()
    {
        if (selectedObject != null && Input.GetKeyDown(KeyCode.R))
        {
            selectedObject.transform.Rotate(0f, 45f, 0f);
            Debug.Log($"Rotated {selectedObject.name}");
        }
    }
}
