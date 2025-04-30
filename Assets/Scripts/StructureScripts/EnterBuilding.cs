using Manager;
using PlayerStatus;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    public string sceneToLoad = "InsideScene"; // Set in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = FindObjectOfType<Inventory>();
        var equipUI = FindObjectOfType<EquipmentUIManager>();
        var playerStats = other.GetComponent<PlayerStatusManager>();
        var transitionManager = FindObjectOfType<SceneTransitionManager>();

        if (GameManagerPersistent.Instance != null && inv != null && equipUI != null && playerStats != null)
        {
            GameManagerPersistent.Instance.SaveGameData(inv, equipUI, playerStats);

            if (transitionManager != null)
            {
                transitionManager.LoadSceneWithFade(sceneToLoad);
            }
            else
            {
                Debug.LogError("SceneTransitionManager not found in the scene.");
            }
        }
    }
}
