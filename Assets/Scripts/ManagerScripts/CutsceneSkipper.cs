using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneSkipper : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public GameObject skipTextObject; // Assign the TMP "Press any key to skip" UI Text
    public string nextSceneName = "MainScene"; // Replace with your actual next scene name

    private bool hasStarted = false;
    private bool hasSkipped = false;

    void Start()
    {
        if (skipTextObject != null)
            skipTextObject.SetActive(false);
    }

    void Update()
    {
        // Show skip text when cutscene starts playing
        if (!hasStarted && timelineDirector.state == PlayState.Playing)
        {
            hasStarted = true;
            if (skipTextObject != null)
                skipTextObject.SetActive(true);
        }

        // Detect any key press to skip and load next scene
        if (hasStarted && !hasSkipped && Input.anyKeyDown)
        {
            hasSkipped = true;

            if (skipTextObject != null)
                skipTextObject.SetActive(false);

            timelineDirector.Stop(); // Stop the Timeline
            SceneManager.LoadScene(nextSceneName); // Load the next scene
        }
    }
}
