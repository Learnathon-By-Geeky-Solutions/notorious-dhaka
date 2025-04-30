using UnityEngine;

public class MissionMarkerFollow : MonoBehaviour
{
    public Transform player;          // Player (Rivan)
    public Transform missionTarget;   // Mission object (like Mission 01)
    public float mapRange = 50f;       // How far the camera can show

    public float followHeight = 3f;    // Height to float above ground
    public float edgeDistance = 30f;   // Distance at which to stop moving further from player

    void Update()
    {
        if (player == null || missionTarget == null)
            return;

        Vector3 toTarget = missionTarget.position - player.position;
        toTarget.y = 0; // We only care about XZ plane

        if (toTarget.magnitude < mapRange)
        {
            // If target is within map range, show normal position
            transform.position = missionTarget.position + Vector3.up * followHeight;
        }
        else
        {
            // If too far, clamp the marker to map edge
            Vector3 clampedPos = player.position + toTarget.normalized * edgeDistance;
            clampedPos.y = missionTarget.position.y + followHeight;
            transform.position = clampedPos;
        }

        // Rotate the arrow to point toward mission
        float angle = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90f, angle, 0f);
    }
}
