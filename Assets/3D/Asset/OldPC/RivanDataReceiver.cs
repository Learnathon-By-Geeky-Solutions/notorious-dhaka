using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class RivanDataReceiver : MonoBehaviour
{

    public RoyData rawDataFromRoy;
    public bool dataProcessed = false;

 
    public GameObject dataPanel; 
    public TMP_Text dataText;       

    private void Awake()
    {
        rawDataFromRoy = new RoyData
        {
            dataPoints = new string[]
            {
                "Scavenger Location",
                "Power Core Coordinates",
                "Encrypted Signal Logs"
            }
        };

        if (dataPanel != null)
            dataPanel.SetActive(false);
    }

    public void ProcessData()
    {
        if (rawDataFromRoy != null && !dataProcessed)
        {
            dataProcessed = true;

            string displayText = "DATA EXTRACTED:\n\n";
            foreach (string entry in rawDataFromRoy.dataPoints)
            {
                displayText += "→ " + entry + "\n";
            }

            if (dataText != null)
                dataText.text = displayText;

            if (dataPanel != null)
                dataPanel.SetActive(true);
        }
        else if (dataProcessed)
        {
            Debug.Log("Data already processed.");
        }
        else
        {
            Debug.LogWarning("No data available to process.");
        }
    }

    public void CloseDataPanel()
    {
        if (dataPanel != null)
        {
            dataPanel.SetActive(false);
            Debug.Log("Data panel closed.");
        }
    }
}
