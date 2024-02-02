using UnityEngine;

/// <summary>
/// Auto destroyer is enabled for comet tail whwn comet collides a celestial body.
/// </summary>
public class CometTailAutodestroy : MonoBehaviour
{
    /* ==================================================
     * ==================== VARIABLES ===================
     * ================================================== */
    float timer;        //Timer (in seconds) to destroy this comet tail.

    /* ==================================================
     * ===================== METHODS ====================
     * ================================================== */

    void Start()
    {
        ParticleSystem particleSystemCmpt = GetComponent<ParticleSystem>();
        timer = 2 * particleSystemCmpt.main.duration;
    }

    void FixedUpdate()
    {
        if(timer > 0.0f)
            timer -= Time.fixedDeltaTime;
        else
            Destroy(this.gameObject);
    }
}
