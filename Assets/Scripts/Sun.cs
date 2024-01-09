using UnityEngine;

public class Sun : SphericalCelestialObject
{
    /* ================================================== 
     * ============= INSPECTOR'S PROPERTIES =============
     * ================================================== */
    [SerializeField]
    [Tooltip("Temperature (in K)")]
    float temperature;              //Temperature (in Kelvin) of Sun.

    /* ================================================== 
     * =================== VARIABLES ====================
     * ================================================== */
    Light lightCmpt;

    protected new void Start()
    {
        base.Start();
        transform.localScale /= 5;
        
        lightCmpt = GetComponent<Light>();
        lightCmpt.range = ScaleConverter.ScaleLength(1.1f * 4.498408f * Mathf.Pow(10, 9) * (1.0f + 0.008678f));
    }

    /* ==================================================
     * ================= PUBLIC METHODS =================
     * ================================================== */

    public float GetTemperature()
    {
        return this.temperature;
    }
}
