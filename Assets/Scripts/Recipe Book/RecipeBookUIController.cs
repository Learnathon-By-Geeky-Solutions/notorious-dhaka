using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookUIController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject recipePanel;
    public Button nextButton;
    public Button prevButton;
    public Button closeButton;
    public GameObject openRecipeButton; // 👈 Reference to the open button

    [Header("Pages (Manual TMPs)")]
    public List<GameObject> recipePages; // Assign in order: Page 0, Page 1, Page 2...

    private int currentPage = 0;

    void Start()
    {
        recipePanel.SetActive(false);

        // ✅ REMOVE this line, it's causing the bug
        // if (openRecipeButton != null) openRecipeButton.SetActive(true);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextPage);

        if (prevButton != null)
            prevButton.onClick.AddListener(PreviousPage);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
    }


    public void ToggleRecipeBook()
    {
        bool shouldShow = !recipePanel.activeSelf;
        recipePanel.SetActive(shouldShow);

        if (openRecipeButton != null)
            openRecipeButton.SetActive(!shouldShow); // 👈 hide or show the open button

        if (shouldShow)
        {
            currentPage = 0;
            ShowPage(currentPage);
        }
    }

    private void ShowPage(int pageIndex)
    {
        if (recipePages.Count == 0) return;

        for (int i = 0; i < recipePages.Count; i++)
        {
            recipePages[i].SetActive(i == pageIndex);
        }

        prevButton.interactable = pageIndex > 0;
        nextButton.interactable = pageIndex < recipePages.Count - 1;
    }

    private void NextPage()
    {
        if (currentPage < recipePages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    private void ClosePanel()
    {
        recipePanel.SetActive(false);

        if (openRecipeButton != null)
            openRecipeButton.SetActive(true); // 👈 show the open button again
    }
}
