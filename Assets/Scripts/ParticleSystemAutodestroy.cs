using UnityEngine;

/// <summary>
/// Auto destroyer is enabled for a particle system that is occured.
/// </summary>
public class ParticleSystemAutodestroy : MonoBehaviour
{
    /* ==================================================
     * ==================== VARIABLES ===================
     * ================================================== */
    float timer;        //Timer (in seconds) to destroy this particle system object.

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
