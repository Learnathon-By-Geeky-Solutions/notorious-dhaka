using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KickHitbox kickHitbox;

    public void StartKick() { kickHitbox.StartKick(); }
    public void EndKick() { kickHitbox.EndKick(); }
}
