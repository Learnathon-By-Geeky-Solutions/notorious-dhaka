using UnityEngine;

public class Roy : MonoBehaviour
{
    public RoyData collectedData;

    private void Awake()
    {
        collectedData = new RoyData
        {
            dataPoints = new string[] { "Scavenger Base Location", "Energy Source Found", "Encrypted Map Data" }
        };
    }
}
