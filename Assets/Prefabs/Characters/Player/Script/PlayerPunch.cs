using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public Animator animator;
    public PunchHitbox punchHitbox;
    public ParticleSystem puncheffect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Punch");
        }
    }

    public void ActivatePunchHitbox()
    {
        punchHitbox.ActivateHitboxAsync();
        puncheffect.Play();
    }
}
