using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;
using PlayerStatus;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByIndex(int sceneIndex)
    {
        SaveCurrentGameState();
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void SaveCurrentGameState()
    {
        var inv = FindObjectOfType<Inventory>();
        var equipUI = FindObjectOfType<EquipmentUIManager>();
        var playerStats = FindObjectOfType<PlayerStatusManager>();

        if (inv != null && equipUI != null && playerStats != null && GameManagerPersistent.Instance != null)
        {
            GameManagerPersistent.Instance.SaveGameData(inv, equipUI, playerStats);
        }
    }
}
