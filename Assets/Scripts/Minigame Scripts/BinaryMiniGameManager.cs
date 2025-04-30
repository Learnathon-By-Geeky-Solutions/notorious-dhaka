using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BinaryMiniGameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject binaryGamePanel;
    public GameObject bitButtonPrefab;
    public Transform bitContainer;
    public RivanDataReceiver dataReceiver;

    [Header("Timer UI")]
    public TMP_Text timerText;           // ✅ Drag in Inspector
    public TMP_Text resultText;          // ✅ Drag in Inspector
    public GameObject resultPanel;       // ✅ Drag in Inspector

    [Header("Bit Colors")]
    public Color defaultColor = Color.green;
    public Color clickedColor = Color.white;

    private List<TMP_Text> bitTexts = new List<TMP_Text>();
    private string originalBits;
    private bool gameWon = false;
    private float timeLeft = 20f;
    private bool timerRunning = false;

    private void Start()
    {
        binaryGamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void StartBinaryGame()
    {
        Time.timeScale = 0f;
        binaryGamePanel.SetActive(true);
        bitTexts.Clear();
        gameWon = false;
        timeLeft = 20f;
        timerRunning = true;

        resultPanel.SetActive(false);
        timerText.text = "20";

        foreach (Transform child in bitContainer)
        {
            Destroy(child.gameObject);
        }

        GenerateRandomBits();

        // Start the timer coroutine
        StartCoroutine(TimerCountdown());
    }

    void GenerateRandomBits()
    {
        originalBits = "";

        for (int i = 0; i < 32; i++)
        {
            int bit = Random.Range(0, 2);
            originalBits += bit.ToString();

            GameObject newBit = Instantiate(bitButtonPrefab, bitContainer);
            TMP_Text text = newBit.GetComponentInChildren<TMP_Text>();
            text.text = bit.ToString();
            text.color = defaultColor;

            int index = i;
            newBit.GetComponent<Button>().onClick.AddListener(() => ToggleBit(text, index));

            bitTexts.Add(text);
        }
    }

    void ToggleBit(TMP_Text bitText, int index)
    {
        if (gameWon || !timerRunning) return;

        bitText.text = bitText.text == "0" ? "1" : "0";
        bitText.color = clickedColor;

        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        for (int i = 0; i < 32; i++)
        {
            if (originalBits[i].ToString() == bitTexts[i].text)
                return;
        }

        OnGameWon();
    }

    void OnGameWon()
    {
        gameWon = true;
        timerRunning = false;

        binaryGamePanel.SetActive(false);
        Time.timeScale = 1f;

        if (dataReceiver != null)
            dataReceiver.ProcessData();

        resultPanel.SetActive(true);
        resultText.text = "Completed Mission 01";
    }

    IEnumerator TimerCountdown()
    {
        while (timerRunning && timeLeft > 0f)
        {
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSecondsRealtime(1f); // Use unscaled time
            timeLeft -= 1f;
        }

        if (!gameWon)
        {
            timerRunning = false;
            Time.timeScale = 1f;
            binaryGamePanel.SetActive(false);

            resultPanel.SetActive(true);
            resultText.text = "You Lost";

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
#else
            Application.Quit(); // Quit in build
#endif
        }
    }

    public void CancelGame()
    {
        timerRunning = false;
        Time.timeScale = 1f;
        binaryGamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }
}
