using UnityEngine;

/// <summary>
/// A celestial object described by a spherical mesh.
/// </summary>
public class SphericalCelestialObject : CelestialObject
{
    /* ==================================================
     * ============= INSPECTOR'S PROPERTIES ============= 
     * ================================================== */
    [SerializeField]
    [Tooltip("Equatorial radius (in km)")]
    float equatorialRadius;             //Equatorial radius (in km) of celestial object.

    [SerializeField]
    float flattening;                   //Flattening of object celestial.

    protected new void Start()
    {
        base.Start();
        this.transform.localScale = ScaleConverter.ScaleSphericalCelestialObject(equatorialRadius, (1 - flattening) * equatorialRadius);
    }

    /// <summary>
    /// Return equatorial radius of spherical celestial object.
    /// </summary>
    /// <returns>Equatorial radiues of spherical celestial object.</returns>
    public float GetEquatorialRadius()
    {
        return this.transform.localScale.x / 2.0f;
    }

    /// <summary>
    /// Return polar radius of spherical celestial object.
    /// </summary>
    /// <returns>Polar radiues of spherical celestial object.</returns>
    public float GetPolarRadius()
    {
        return this.transform.localScale.y / 2.0f;
    }
}
