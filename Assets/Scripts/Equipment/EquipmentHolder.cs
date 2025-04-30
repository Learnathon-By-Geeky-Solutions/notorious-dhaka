using UnityEngine;

public class EquipmentHolder : MonoBehaviour
{
    public Transform rightHand;
    public GameObject equippedObject;

    public void Equip(GameObject obj)
    {
        if (equippedObject != null) Destroy(equippedObject);
        equippedObject = Instantiate(obj, rightHand);
        equippedObject.transform.localPosition = Vector3.zero;
        equippedObject.transform.localRotation = Quaternion.identity;

        // Disable collision with player
        Collider itemCol = equippedObject.GetComponent<Collider>();
        Collider playerCol = GetComponentInParent<Collider>();
        if (itemCol != null && playerCol != null)
            Physics.IgnoreCollision(itemCol, playerCol, true);
    }

    public GameObject Unequip()
    {
        if (equippedObject == null) return null;

        GameObject obj = equippedObject;
        Collider itemCol = obj.GetComponent<Collider>();
        Collider playerCol = GetComponentInParent<Collider>();
        if (itemCol != null && playerCol != null)
            Physics.IgnoreCollision(itemCol, playerCol, false);

        equippedObject = null;
        Destroy(obj);
        return obj;
    }

    public bool HasItem(string itemName) =>
        equippedObject != null && equippedObject.name.Contains(itemName);
}
