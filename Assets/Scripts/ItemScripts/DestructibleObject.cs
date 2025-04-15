using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject[] objectsToSpawn;  // Prefabs to spawn after destruction
    public Transform spawnPoint;         // Optional spawn location
    public float destructionDelay = 1f;  // Time to wait after Rivan animation
}
