using UnityEngine;
using Scriptables;

public class UseItemManager : MonoBehaviour
{
    public void UseItem(Items item)
    {
        Debug.Log($"Used: {item.itemName}");

        // Implement your actual item effect logic
        if (item.itemName.ToLower().Contains("potion"))
        {
            // Heal player
            Debug.Log("Healed player!");
        }
        else
        {
            // Other effects
            Debug.Log("Applied effect!");
        }
    }
}
