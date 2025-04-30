using UnityEngine;
using FirstGearGames.SmoothCameraShaker; // ✅ External camera shake package

public class PlayerPunch : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public PunchHitbox punchHitbox;
    public ParticleSystem punchEffect;
    public ShakeData shakeData; // ✅ Used by SmoothCameraShaker

    [Header("Punch Audio")]
    public AudioSource punchAudio;
    public AudioClip punchClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Punch");

            // ✅ Trigger shake using SmoothCameraShaker
            CameraShakerHandler.Shake(shakeData);
        }
    }

    // 🔥 Called via Animation Event during punch animation
    public void ActivatePunchHitbox()
    {
        // ✅ 1. Activate hitbox
        punchHitbox?.ActivateHitboxAsync();

        // ✅ 2. Play particle effect
        if (punchEffect != null)
            punchEffect.Play();
        else
            Debug.LogWarning("[PlayerPunch] Punch effect is missing.");

        // ✅ 3. Play punch sound
        if (punchAudio != null && punchClip != null)
            punchAudio.PlayOneShot(punchClip);
    }
}
