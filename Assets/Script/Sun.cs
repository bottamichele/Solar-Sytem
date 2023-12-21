using UnityEngine;

public class Sun : SphericalCelestialObject
{
    /* ================================================== 
     * ============= INSPECTOR'S PROPERTIES =============
     * ================================================== */
    [SerializeField]
    [Tooltip("Temperature (in K)")]
    float temperature;              //Temperature (in Kelvin) of Sun.

    protected new void Start()
    {
        base.Start();
        this.transform.localScale /= 5;
    }
}
