using UnityEngine;
using PlayerStatus;
using Manager;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        var inv = FindObjectOfType<Inventory>();
        var equipUI = FindObjectOfType<EquipmentUIManager>();
        var playerStats = FindObjectOfType<PlayerStatusManager>();
        var holder = FindObjectOfType<EquipmentHolder>();

        if (GameManagerPersistent.Instance != null && inv != null && equipUI != null && playerStats != null && holder != null)
        {
            // Apply inventory, equipment, and stats (but skip saved position)
            GameManagerPersistent.Instance.ApplyGameData(inv, equipUI, playerStats, holder);

            // ✅ Manually place the player at the desired spawn point
            playerStats.transform.position = new Vector3(1284.01f, 50.44f, 340.18f);
            playerStats.transform.rotation = Quaternion.Euler(0f, 183.693f, 0f);

            Debug.Log("[SceneInitializer] Player positioned using fixed coordinates.");
        }
        else
        {
            Debug.LogError("[SceneInitializer] One or more required components are missing.");
        }
    }
}
