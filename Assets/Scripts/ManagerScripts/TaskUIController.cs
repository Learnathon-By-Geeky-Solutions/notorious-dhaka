using UnityEngine;

public class TaskUIController : MonoBehaviour
{
    [Header("Assign your Task UI panel here")]
    public GameObject taskPanel;

    private bool isVisible = false;

    void Start()
    {
        if (taskPanel != null)
        {
            taskPanel.SetActive(false); // Hide on start
        }
        else
        {
            Debug.LogWarning("[TaskUIController] Task Panel is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTaskPanel();
        }
    }

    private void ToggleTaskPanel()
    {
        if (taskPanel == null)
        {
            Debug.LogError("[TaskUIController] Task Panel reference is missing.");
            return;
        }

        isVisible = !isVisible;
        taskPanel.SetActive(isVisible);
    }
}
