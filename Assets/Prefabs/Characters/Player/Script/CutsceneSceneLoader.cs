using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad; // Assign in Inspector

    // Call this when the cutscene ends
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Optional: Delay version
    public void LoadSceneAfterDelay(float delay)
    {
        Invoke(nameof(LoadNextScene), delay);
    }
}
