using UnityEngine;

public class SceneIntroManager : MonoBehaviour
{
    public GameObject firstUIPanel;
    public GameObject secondUIPanel;

    [Header("Buttons to Show After Intro")]
    public GameObject honourButton;           // ✅ Button 1
    public GameObject secondaryGameplayButton; // ✅ Button 2 (e.g. inventory, map, etc.)

    private bool isFirstUIShowing = true;
    private bool isSecondUIShowing = false;
    private bool introFinished = false;

    void Start()
    {
        Time.timeScale = 0f;

        if (firstUIPanel != null)
            firstUIPanel.SetActive(true);

        if (secondUIPanel != null)
            secondUIPanel.SetActive(false);

        if (honourButton != null)
            honourButton.SetActive(false);

        if (secondaryGameplayButton != null)
            secondaryGameplayButton.SetActive(false);
    }

    void Update()
    {
        if (introFinished)
            return;

        if (Input.anyKeyDown)
        {
            if (isFirstUIShowing)
            {
                if (firstUIPanel != null)
                    firstUIPanel.SetActive(false);

                if (secondUIPanel != null)
                    secondUIPanel.SetActive(true);

                isFirstUIShowing = false;
                isSecondUIShowing = true;
            }
            else if (isSecondUIShowing)
            {
                if (secondUIPanel != null)
                    secondUIPanel.SetActive(false);

                isSecondUIShowing = false;
                introFinished = true;

                Time.timeScale = 1f;

                // ✅ Show both buttons after intro ends
                if (honourButton != null)
                    honourButton.SetActive(true);

                if (secondaryGameplayButton != null)
                    secondaryGameplayButton.SetActive(true);
            }
        }
    }
}
