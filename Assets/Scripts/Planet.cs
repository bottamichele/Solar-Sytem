using UnityEngine;

/// <summary>
/// A planet.
/// </summary>
public class Planet : SphericalCelestialObject
{
    /* ================================================== 
     * ============= INSPECTOR'S PROPERTIES =============
     * ================================================== */
    [Header("Orbital charateristics")]

    [SerializeField]
    [Tooltip("Semi-major axis of orbit (in km).")]
    float semiMajorAxis;                //Semi-major axis of orbital (in km).

    [SerializeField]
    [Tooltip("Eccentricity of orbit.")]
    float eccentricity;                 //Eccentricity of orbital.

    [SerializeField]
    [Tooltip("Longitudine of ascending node (in degree) of orbit.")]
    float ascendingNode;                //Longitudine of ascending node (in degree).

    [SerializeField]
    [Tooltip("Argument of perihelion (in degree) of orbit.")]
    float argumentPerihelion;            //Argument of perihelion (in degree).

    [SerializeField]
    [Tooltip("Oribital inclination (in degree).")]
    float inclination;                  //Orbital inclination (in degree).

    [SerializeField]
    [Tooltip("Star this planet orbits.")]
    CelestialObject star;               //Star this planet orbits.

    /* ==================================================
     * ==================== METHODS =====================
     * ================================================== */

    protected new void Start()
    {
        base.Start();

        //Generate the orbit.
        Orbit.GenerateEllipseOrbit(this,
                                   ScaleConverter.ScaleLength(semiMajorAxis),
                                   eccentricity,
                                   ascendingNode,
                                   argumentPerihelion,
                                   inclination,
                                   star);
    }
}
