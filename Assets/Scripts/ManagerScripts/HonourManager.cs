using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HonourManager : MonoBehaviour
{
    [System.Serializable]
    public class Honour
    {
        public string title;
        public string targetTag;
        public int requiredKills;
        [HideInInspector] public bool isUnlocked = false;
    }

    public List<Honour> honoursList = new List<Honour>();
    private Dictionary<string, int> killCounts = new Dictionary<string, int>();

    public static HonourManager Instance;

    [Header("Honour Panel Settings")]
    public GameObject honourPanel;
    public Transform honourListParent;
    public GameObject honourEntryPrefab;

    [Header("Honour Popup Settings")]
    public GameObject honourPopupPanel;             // 🔥 New: The popup panel
    public TextMeshProUGUI honourPopupText;          // 🔥 New: Text inside the popup
    public float popupDuration = 2f;                 // 🔥 How long the popup stays

    [Header("Development Settings")]
    public bool clearAllHonoursOnStart = false;

    private Coroutine popupCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (clearAllHonoursOnStart)
        {
            ClearAllHonours();
        }

        LoadHonours();
    }

    public void RegisterKill(string enemyTag)
    {
        if (!killCounts.ContainsKey(enemyTag))
            killCounts[enemyTag] = 0;

        killCounts[enemyTag]++;
        Debug.Log($"[HonourManager] {enemyTag} kills: {killCounts[enemyTag]}");

        foreach (var honour in honoursList)
        {
            if (!honour.isUnlocked && honour.targetTag == enemyTag && killCounts[enemyTag] >= honour.requiredKills)
            {
                UnlockHonour(honour);
            }
        }
    }

    private void UnlockHonour(Honour honour)
    {
        honour.isUnlocked = true;
        Debug.Log($"[HonourManager] Unlocked Honour: {honour.title}!");

        PlayerPrefs.SetInt($"HonourUnlocked_{honour.title}", 1);
        PlayerPrefs.Save();

        RefreshHonourPanel();
        ShowHonourPopup();  // 🔥 Show popup after unlocking
    }

    private void LoadHonours()
    {
        foreach (var honour in honoursList)
        {
            if (PlayerPrefs.GetInt($"HonourUnlocked_{honour.title}", 0) == 1)
            {
                honour.isUnlocked = true;
                Debug.Log($"[HonourManager] Loaded unlocked Honour: {honour.title}");
            }
        }
    }

    public void ToggleHonourPanel()
    {
        if (honourPanel == null)
            return;

        bool isActive = honourPanel.activeSelf;
        honourPanel.SetActive(!isActive);

        if (!isActive)
            RefreshHonourPanel();
    }

    public void RefreshHonourPanel()
    {
        if (honourListParent == null || honourEntryPrefab == null)
            return;

        foreach (Transform child in honourListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var honour in honoursList)
        {
            if (honour.isUnlocked)
            {
                GameObject entry = Instantiate(honourEntryPrefab, honourListParent);
                TextMeshProUGUI text = entry.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = honour.title;
                }
            }
        }
    }

    // 🔥 New: Show Honour Popup
    private void ShowHonourPopup()
    {
        if (honourPopupPanel == null || honourPopupText == null)
            return;

        if (popupCoroutine != null)
            StopCoroutine(popupCoroutine);

        popupCoroutine = StartCoroutine(PopupRoutine());
    }

    private IEnumerator PopupRoutine()
    {
        honourPopupPanel.SetActive(true);
        honourPopupText.text = "New Honour Unlocked!"; // ✅ Simple message

        yield return new WaitForSeconds(popupDuration);

        honourPopupPanel.SetActive(false);
    }

    private void ClearAllHonours()
    {
        foreach (var honour in honoursList)
        {
            PlayerPrefs.DeleteKey($"HonourUnlocked_{honour.title}");
        }
        PlayerPrefs.Save();
        Debug.Log("[HonourManager] All Honour PlayerPrefs cleared.");
    }
}
