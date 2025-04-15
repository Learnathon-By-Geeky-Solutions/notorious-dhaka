using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class BinaryMiniGameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject binaryGamePanel;
    public GameObject bitButtonPrefab;
    public Transform bitContainer;
    public RivanDataReceiver dataReceiver;

    [Header("Bit Colors")]
    public Color defaultColor = Color.green;
    public Color clickedColor = Color.white;

    private List<TMP_Text> bitTexts = new List<TMP_Text>();
    private string originalBits;
    private bool gameWon = false;

    private void Start()
    {
        binaryGamePanel.SetActive(false);
    }

    public void StartBinaryGame()
    {
        Time.timeScale = 0f;
        binaryGamePanel.SetActive(true);
        bitTexts.Clear();
        gameWon = false;

        foreach (Transform child in bitContainer)
        {
            Destroy(child.gameObject);
        }

        GenerateRandomBits();
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
        if (gameWon) return;

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
        Debug.Log("Binary puzzle solved!");

        binaryGamePanel.SetActive(false);
        Time.timeScale = 1f;

        if (dataReceiver != null)
        {
            dataReceiver.ProcessData();
        }
    }

    public void CancelGame()
    {
        Time.timeScale = 1f;
        binaryGamePanel.SetActive(false);
    }
}
