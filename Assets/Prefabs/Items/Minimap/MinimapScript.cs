using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public GameObject player;
    public float cameraHeight = 100f; // Editable Y position

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(
                player.transform.position.x + 3,
                cameraHeight,
                player.transform.position.z - 1
            );
        }
    }
}
