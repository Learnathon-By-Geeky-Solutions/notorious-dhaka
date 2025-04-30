using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;  // Assign the FadePanel's CanvasGroup in Inspector
    public float fadeDuration = 1f;

    private void Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Pause gameplay
        Time.timeScale = 0f;

        // Fade using unscaled time
        yield return StartCoroutine(Fade(0f, 1f, useUnscaledTime: true));

        // Delay before switching (also unscaled)
        yield return new WaitForSecondsRealtime(0.2f);

        // Resume time before loading (optional — new scene will reset it anyway)
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, bool useUnscaledTime = false)
    {
        float elapsed = 0f;

        fadeCanvasGroup.blocksRaycasts = true;

        while (elapsed < fadeDuration)
        {
            elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}
